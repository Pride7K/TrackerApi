using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TrackerApi;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Login.ViewModel;
using TrackerApi.Services.UserService.ViewModel;
using Xunit;

namespace TrackerApiTest
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _client;

        protected IntegrationTest()
        {
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder((builder) =>
            {

                builder.ConfigureServices((services) =>
                {
                    var descriptor = services.SingleOrDefault(
                                    d => d.ServiceType ==
                                        typeof(DbContextOptions<AppDbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });



            _client = factory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
           if(_client.DefaultRequestHeaders.Authorization == null)
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetJwtToken());
            }
        }

        private async Task<string> GetJwtToken()
        {
            var responseUser = await _client.PostAsJsonAsync("/v1/users", new CreateUserViewModel
            {
                Name = "teste",
                Email = "teste@gmail.com",
                Password = "teste",
            });

            var user = await responseUser.Content.ReadFromJsonAsync<User>();

            var responseAuthUser = await _client.PostAsJsonAsync("/v1/login/user", new AuthenticateUserViewModel
            {
                Email = user.Email,
                Password = user.Password
            });

            var responseConverted = await responseAuthUser.Content.ReadFromJsonAsync<TokenResponse>();


            return responseConverted.token;
        }

        public struct TokenResponse
        {
            public string token { get; set; }
        }
    }
}
