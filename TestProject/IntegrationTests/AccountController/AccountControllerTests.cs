using Azure;
using Castle.Components.DictionaryAdapter;
using Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace TestProject.IntegrationTests.AccountController
{
    public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        

        public AccountControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
           
        }
        [Fact]
        public async Task Register_ReturnsSuccess()
        {
            var newUser = new RegisterDTO
            {
                
                Email= "adam@test.com",
                Password = "Pa$$123",
                UserName = "userName"
            };
            var response = await _client.PostAsJsonAsync("api/Account/Register", newUser);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<RegisterDTO>(responseString);

            var registeredUser = new LoginDTO
            {
                Email = "adam@test.com",
                Password = "Pa$$123"
            };
            var loginResponse = await _client.PostAsJsonAsync("api/Account/Login", registeredUser);
            loginResponse.EnsureSuccessStatusCode();
            Assert.NotNull(user);
            Assert.Equal(newUser.Email, user.Email);

        }
    }
}
