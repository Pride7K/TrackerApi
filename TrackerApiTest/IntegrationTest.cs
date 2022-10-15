using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

                    services.RemoveAll(descriptor.ServiceType);

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

            _client.DefaultRequestHeaders.Authorization = await GetJwtToken();
        }

        private async Task<User> GetAdminUser()
        {
            User user = null;

            var adminUser = new CreateUserViewModel
            {
                Name = "teste",
                Email = "teste@gmail.com",
                Password = "teste",
            };

            var responseUser = await _client.PostAsJsonAsync("/v1/users", adminUser);

            if (responseUser.IsSuccessStatusCode)
                user = await responseUser.Content.ReadFromJsonAsync<User>();
            else
            {
                user = new User
                {
                    Email = adminUser.Email,
                    Name = adminUser.Name,
                    Password = adminUser.Password
                };
            }


            return user;
        }

        private async Task<AuthenticationHeaderValue> GetJwtToken()
        {

            var user = await GetAdminUser();

            var responseAuthUser = await _client.PostAsJsonAsync("/v1/login/user", new AuthenticateUserViewModel
            {
                Email = user.Email,
                Password = user.Password
            });

            var responseConverted = await responseAuthUser.Content.ReadFromJsonAsync<TokenResponse>();

            return new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", responseConverted.token);
        }

        public struct TokenResponse
        {
            public string token { get; set; }
        }
    }
}
