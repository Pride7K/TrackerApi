using FluentAssertions;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TrackerApi.Models;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Services.UserService.ViewModel;
using Xunit;

namespace TrackerApiTest
{
    public class UserControllerTest : IntegrationTest
    {

        [Fact]
        public async Task GetAll_ReturnsListUsersResponse()
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
            await CreateUserAsync(new CreateUserViewModel
            {
                Email = "sdsdsdsdds@gmail.com",
                Password = "asasasa",
                Name = "ashasjas"
            });

            var response = await _client.GetAsync("/v1/users/skip/0/take/25");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<GetUsersViewModel>()).Users.Should().NotBeEmpty().And.HaveCount(1);


        }

        [Fact]
        public async Task Get_SpecificUser_WhenUsersExistsInDataBase()
        {
            await AuthenticateAsync();
            var user = await CreateUserAsync(new CreateUserViewModel
            {
                Email = "sdsdsdsdds@gmail.com",
                Password = "asasasa",
                Name = "ashasjas"
            });

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
    }
}
