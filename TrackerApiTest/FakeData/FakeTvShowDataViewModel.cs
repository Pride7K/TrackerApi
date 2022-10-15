using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerApi.Services.TvShowService.ViewModel;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApiTest.FakeData
{
    public static class FakeTvShowDataViewModel
    {
        public static Faker<CreateTvShowViewModel> _fakerCreateTvShow = new Faker<CreateTvShowViewModel>()
            .RuleFor(t => t.Title, f => f.Name.FirstName())
            .RuleFor(t => t.Description, f => f.Name.FullName())
            .RuleFor(t => t.StillGoing, true);

        public static Faker<PutTvShowViewModel> _fakerPutTvShow = new Faker<PutTvShowViewModel>()
    .RuleFor(t => t.Title, f => f.Name.FirstName())
    .RuleFor(t => t.Description, f => f.Name.FullName())
    .RuleFor(t => t.Available, f => f.Random.Bool());
    }
}
