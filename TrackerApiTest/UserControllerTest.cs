using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackerApi.Controllers;
using TrackerApi.Models;
using TrackerApi.Services.UserService;
using TrackerApi.Services.UserService.ViewModel;
using Xunit;

namespace TrackerApiTest
{
    public class UserControllerTest
    {

        [Fact]
        public async Task GetUsers_ShouldReturn_OK()
        {
            Moq.Mock<IUserService> mock = new Moq.Mock<IUserService>();
            mock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<GetUsersViewModel>()));

            var userController = new UserController(mock.Object);


            var result = ((IStatusCodeActionResult)userController.GetAsync(0, 25).Result).StatusCode.Value;

            result.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public void GetUsers_ShouldReturn_BadRequest()
        {
            Moq.Mock<IUserService> mock = new Moq.Mock<IUserService>();
            mock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<GetUsersViewModel>()));

            var userController = new UserController(mock.Object);


            var result = ((IStatusCodeActionResult)userController.GetAsync(0, 10000).Result).StatusCode.Value;

            result.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public void GetUsers_ShouldReturn_Unathorized()
        {
            Moq.Mock<IUserService> mock = new Moq.Mock<IUserService>();
            mock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<GetUsersViewModel>()));

            var userController = new UserController(mock.Object);


            var result = ((IStatusCodeActionResult)userController.GetAsync(0, 10000).Result).StatusCode.Value;

            result.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public void MustCreateUser()
        {
            var cardMock = new Mock<User>();

            var testObject = new CreateUserViewModel()
            {
                Name = "sdsd",
                Email = "sdsdsd",
                Password = "sdsdsd"
            };

            Moq.Mock<IUserService> mock = new Moq.Mock<IUserService>();
            mock.Setup(x => x.Create(It.IsAny<CreateUserViewModel>())).Returns(Task.FromResult(cardMock.Object));

            var userController = new UserController(mock.Object);

            var result = (Microsoft.AspNetCore.Mvc.CreatedResult)userController.PostAsync(testObject).Result;

            Assert.True(result.StatusCode == 201);
        }
    }
}
