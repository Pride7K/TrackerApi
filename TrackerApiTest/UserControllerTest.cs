using Bogus;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Services.UserService.ViewModel;
using TrackerApiTest.FakeData;
using Xunit;

namespace TrackerApiTest
{
    public class UserControllerTest : IntegrationTest
    {

        [Fact]
        public async Task Get_AllUsers_ShouldReturnListUsersResponse()
        {

            await AuthenticateAsync();

            var response = await _client.GetAsync("/v1/users/skip/0/take/25");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadFromJsonAsync<GetUsersViewModel>().Should().NotBeNull();
        }

        protected async Task<User> CreateUserAsync(CreateUserViewModel model)
        {
            var response = await _client.PostAsJsonAsync("/v1/users", model);

            return await response.Content.ReadFromJsonAsync<User>();
        }

        [Fact]
        public async Task Get_ReturnsUsers_WhenUsersExistsInDataBase()
        {
            await AuthenticateAsync();
            var user = await CreateUserAsync(FakeUserDataViewModel._fakerCreateUser.Generate());

            var response = await _client.GetAsync("/v1/users/skip/0/take/25");
            var users = (await response.Content.ReadFromJsonAsync<GetUsersViewModel>()).Users;

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            users.Should().NotBeEmpty().And.HaveCountGreaterThan(0);
            users.FirstOrDefault(t => t.Id == user.Id).Should().NotBeNull();


        }

        [Fact]
        public async Task Get_SpecificUser_WhenUsersExistsInDataBase()
        {
            await AuthenticateAsync();
            var user = await CreateUserAsync(FakeUserDataViewModel._fakerCreateUser.Generate());

            var response = await _client.GetAsync($"/v1/users/{user.Id}");
            var responseUser = await response.Content.ReadFromJsonAsync<User>();


            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseUser.Id.Should().Be(user.Id);
            responseUser.Email.Should().Be(user.Email);

        }

        [Fact]
        public async Task Post_User_ShouldFailValidation()
        {
            await AuthenticateAsync();

            var model = new CreateUserViewModel
            {
                Email = "sdsdsdsdds",
                Password = "asasasa",
                Name = "ashasjas"
            };

            var response = await _client.PostAsJsonAsync("/v1/users", model);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [Theory]
        [InlineData("/v1/users/skip/0/take/25")]
        [InlineData("/v1/users/2")]
        public async Task Get_SpecificRoutes_ShouldFailWhenNotAuthorized(string route)
        {
            var response =  await _client.GetAsync(route);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }


        [Fact]
        public async Task Post_User_ShouldNotCreateIfUserExists()
        {
            await AuthenticateAsync();

            var model = FakeUserDataViewModel._fakerCreateUser.Generate();

            var responseFirstCreation = await _client.PostAsJsonAsync("/v1/users", model);

            var responseSecondCreation = _client.PostAsJsonAsync("/v1/users", model);


            var responseSecondCreationMessage = await responseSecondCreation.Result.Content.ReadAsStringAsync();

            responseFirstCreation.StatusCode.Should().Be(HttpStatusCode.Created);
            responseSecondCreation.Result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseSecondCreationMessage.Should().Contain("User already Exists!");

        }
    }
}
