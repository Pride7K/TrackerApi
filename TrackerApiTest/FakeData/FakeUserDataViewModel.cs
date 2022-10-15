using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApiTest.FakeData
{
    public static class FakeUserDataViewModel
    {
        public static Faker<CreateUserViewModel> _fakerCreateUser = new Faker<CreateUserViewModel>()
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(u => u.Password, f => Guid.NewGuid().ToString());
    }
}
