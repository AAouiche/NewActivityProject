using Application.Messages;
using Domain.DTO;
using Domain.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.IntegrationTests.MessageController
{
    public class MessagesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private string _testUserToken;
        private Guid _activityId;

        public MessagesControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _testUserToken = GetTestUserToken().Result;
            _activityId = new Guid("12345678-9abc-def0-1234-56789abcdef0");
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
        public async Task Post_CreateMessage_ReturnsSuccess()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testUserToken);

            var newMessage = new Create.Command
            {
                ActivityId = _activityId,
                MessageBody = "gsdfgsfdg"
            };

            var response = await _client.PostAsJsonAsync("/api/messages/create", newMessage);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var messageResult = JsonConvert.DeserializeObject<MessageDTO>(responseString);

            Assert.NotNull(messageResult);
           
            
        }

        

        [Fact]
        public async Task Get_ListMessages_ReturnsSuccess()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testUserToken);

            Guid someId = Guid.NewGuid(); 

            var response = await _client.GetAsync($"/api/messages?Id={_activityId}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var messagesResult = JsonConvert.DeserializeObject<List<MessageDTO>>(responseString);

            Assert.NotNull(messagesResult);
            
           
           
        }
    }
}
