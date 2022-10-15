using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrackerApi.Services.UserService.ViewModel;
using Xunit;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Models;
using TrackerApiTest.FakeData;

namespace TrackerApiTest
{
    public class TvShowControllerTest : IntegrationTest
    {

        protected async Task<TvShow> CreateTvShowAsync(CreateTvShowViewModel model)
        {
            var response = await _client.PostAsJsonAsync("/v1/tvshows", model);

            return await response.Content.ReadFromJsonAsync<TvShow>();
        }
        [Theory]
        [InlineData("/v1/tvshows/skip/0/take/25")]
        [InlineData("/v1/tvshows/recomendations/skip/0/take/25")]
        public async Task Get_ShouldReturnOkWhenNotAuthenticate(string route)
        {

            var response = await _client.GetAsync(route);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/v1/tvshows/skip/0/take/25")]
        [InlineData("/v1/tvshows/recomendations/skip/0/take/25")]
        public async Task Get_ShouldReturnOkWhenAuthenticate(string route)
        {
            await AuthenticateAsync();

            var response = await _client.GetAsync(route);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_SpecificTvShow_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var tvShow = await CreateTvShowAsync(FakeTvShowDataViewModel._fakerCreateTvShow.Generate());

            var response = await _client.GetAsync($"/v1/tvshows/{tvShow.Id}");
            var tvShowResponse = await response.Content.ReadFromJsonAsync<TvShow>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            tvShowResponse.Id.Should().Be(tvShow.Id);
        }

        [Fact]
        public async Task Post_SpecificTvShow_ShouldFailWhenNotAuthenticated()
        {

            var response = await _client.PostAsJsonAsync($"/v1/tvshows", FakeTvShowDataViewModel._fakerCreateTvShow.Generate());

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Post_SpecificTvShow_ShouldReturnCreatedWhenAuthenticated()
        {
            await AuthenticateAsync();

            var response = await _client.PostAsJsonAsync($"/v1/tvshows", FakeTvShowDataViewModel._fakerCreateTvShow.Generate());

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_LoadTvShows_ShouldReturnOk()
        {
            var response = await _client.PostAsync($"/v1/tvshows/load", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Put_SpecificTvShow_ShouldReturnOkWhenAuthenticated()
        {
            await AuthenticateAsync();
            var tvShow = await CreateTvShowAsync(FakeTvShowDataViewModel._fakerCreateTvShow.Generate());

            var response = await _client.PutAsJsonAsync($"/v1/tvshows/{tvShow.Id}", FakeTvShowDataViewModel._fakerPutTvShow.Generate() );
            var tvShowPutResponse = await response.Content.ReadFromJsonAsync<TvShow>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            tvShowPutResponse.Id.Should().Be(tvShow.Id);
            tvShowPutResponse.Title.Should().NotBe(tvShow.Title);
            tvShowPutResponse.Description.Should().NotBe(tvShow.Description);
        }

        [Fact]
        public async Task Put_SpecificTvShow_ShouldReturnUnathorizedWhenNotAuthenticated()
        {
            var response = await _client.PutAsJsonAsync($"/v1/tvshows/1", FakeTvShowDataViewModel._fakerPutTvShow.Generate());

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Delete_SpecificTvShow_ShouldReturnUnathorizedWhenNotAuthenticated()
        {
            var response = await _client.DeleteAsync($"/v1/tvshows/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Delete_SpecificTvShow_ShouldReturnOkWhenAuthenticated()
        {
            await AuthenticateAsync();
            var tvShow = await CreateTvShowAsync(FakeTvShowDataViewModel._fakerCreateTvShow.Generate());


            var response = await _client.DeleteAsync($"/v1/tvshows/{tvShow.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}
