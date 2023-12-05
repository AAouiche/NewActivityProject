using Domain.DTO;
using Domain.Models;
using NewActivityProject;
using Microsoft.AspNetCore.Hosting;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Domain.Validation;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace TestProject.IntegrationTests.ActivityController
{
    public class ActivityControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private string _testUserToken;

        public ActivityControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _testUserToken = GetTestUserToken().Result; 
        }

        private async Task<string> GetTestUserToken()
        {
            
            var loginRequest = new
            {
                Email = "bob@test.com", 
                Password = "Pa$$w0rd"
            };

            
            var response = await _client.PostAsJsonAsync("/api/Account/login", loginRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Could not obtain test user token.");
            }


            var responseString = await response.Content.ReadAsStringAsync();
            var userDto = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(responseString);

            return userDto?.Token;
        }

        [Fact]
        public async Task Post_CreateActivity_WithAuthenticatedUser_ReturnsSuccess()
        {
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testUserToken);

            var newActivity = new Activity {
                Id = Guid.NewGuid(),
                Title = "Sample Activity",
                cancelled = false,
                Description = "This is a sample activity",
                Category = "Sports",
                City = "New York",
                Date = DateTime.UtcNow.AddDays(7),
                Venue = "Central Park",
                
                Messages = new List<Message>(),
                Attendees = new List<ActivityAttendee>()
            };

            var response = await _client.PostAsJsonAsync("/api/activity/Create", newActivity);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var activity = JsonConvert.DeserializeObject<ActivityDTO>(responseString);
            Assert.NotNull(responseString);
            Assert.Equal("Sample Activity", activity.Title);
            Assert.Equal("Sports", activity.Category);
            Assert.Equal("New York", activity.City);


        }
       
        [Fact]
        public async Task Get_ListOfActivities_WithFilters()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testUserToken);

            int pageNumber = 1;
            int pageSize = 5;
            string filter = "all"; 
           

           
            var url = $"/api/Activity/List?pageNumber={pageNumber}&pageSize={pageSize}&filter={filter}";

        
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();


            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var jsonDocument = await JsonDocument.ParseAsync(responseStream);
            var itemsElement = jsonDocument.RootElement.GetProperty("items");
            var activities = System.Text.Json.JsonSerializer.Deserialize<List<ActivityDTO>>(itemsElement.GetRawText());


            Assert.NotNull(activities);

           
            Assert.True(activities.Any());
        }
        


    }
}
