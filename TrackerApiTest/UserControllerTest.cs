﻿using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void Should_Return_UserAlreadyExists()
        {
            var cardMock = new Mock<User>();

            var testObject = new CreateUserViewModel()
            {
                Name = "sdsd",
                Email = "sdsdsd",
                Password = "sdsdsd"
            };

            Moq.Mock<IUserService> mock = new Moq.Mock<IUserService>();
            mock.Setup(x => x.Create(testObject)).Returns(Task.FromResult(cardMock.Object));

            var userController = new UserController(mock.Object);

            var result = (Microsoft.AspNetCore.Mvc.CreatedResult)userController.PostAsync(testObject).Result;

            var result2 = (Microsoft.AspNetCore.Mvc.CreatedResult)userController.PostAsync(testObject).Result;

            Assert.True(result2.StatusCode != 201);
        }
    }
}
