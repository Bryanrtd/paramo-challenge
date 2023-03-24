using System;

using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Services.interfaces;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserControllerUnitTest
    {

        [Fact]
        public async void UserController_CreateUser_ReturnsOK()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(x => x.Create(It.IsAny<UserDto>())).ReturnsAsync(new Result()
            {
                IsSuccess = true,
                Messages = "User Created"
            });
            var userController = new UsersController(mockUserService.Object);

            // Act
            var result = await userController.CreateUser(new UserDto());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Messages);
        }


        [Fact]
        public async void UserController_CreateUser_ThrowsException()
        {
            // Arrange
            var service = new Mock<IUserService>();
            service.Setup(x => x.Create(It.IsAny<UserDto>())).ThrowsAsync(new Exception("Bad format"));
            var userController = new UsersController(service.Object);

            // Act
            var result = await userController.CreateUser(new UserDto());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Bad format", result.Messages);
        }
    }
}
