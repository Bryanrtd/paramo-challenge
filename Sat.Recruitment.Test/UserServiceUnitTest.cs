using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Moq;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repositories.interfaces;
using Sat.Recruitment.Api.Services.implementations;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserServiceUnitTest
    {

        [Fact]
        public void Userervice_MapDtoToUser_ShouldBeOk()
        {
            // Arrange
            var repository = new Mock<IUserRepository>();
            var mappedUser =
                new User
                {
                    Name = "JoseLuis",
                    Email = "josel@gmail.com",
                    Address = "C/ Mirador del Este",
                    Phone = "+8091234567",
                    UserType = UserType.Premium,
                    Money = 27000
                };

            var userDto = new UserDto
            {
                Name = "JoseLuis",
                Email = "josel@gmail.com",
                Address = "C/ Mirador del Este",
                Phone = "+8091234567",
                UserType = "Premium",
                Money = 27000
            };

            var userService = new UserService(repository.Object);

            // Act
            var result = userService.MapDtoToUser(userDto);

            // Assert
            Assert.Equal(mappedUser.Name, result.Name);
            Assert.Equal(mappedUser.Email, result.Email);
        }

        [Fact]
        public void Userervice_MapDtoToUser_ShouldBeFail()
        {
            // Arrange
            var repository = new Mock<IUserRepository>();
            var mappedUser =
                new User
                {
                    Name = "JoseLuis",
                    Email = "josel@gmail.com",
                    Address = "C/ Mirador del Este",
                    Phone = "+8091234567",
                    UserType = UserType.Premium,
                    Money = 27000
                };

            var userDto = new UserDto
            {
                Name = "JoseLuis",
                Email = "josel@gmail.com",
                Address = "C/ Mirador del Este",
                Phone = "+8091234567",
                UserType = "Freemium",
                Money = 27000
            };

            var userService = new UserService(repository.Object);

            // Act
            void act() => userService.MapDtoToUser(userDto);
            ValidationException exception = Assert.Throws<ValidationException>(act);

            // Assert
            Assert.Equal($"UserType {userDto.UserType} is invalid, valid types of user: Normal, SuperUser or Premium", exception.Message);
        }


        [Fact]
        public async void UserService_CreateUser_ReturnsOk()
        {
            // Arrange
            var repository = new Mock<IUserRepository>();
            var users = new List<User>();

            repository.Setup(x => x.GetAllUser()).ReturnsAsync(users);

            var userDto = new UserDto
            {
                Name = "JoseLuis",
                Email = "josel@gmail.com",
                Address = "C/ Mirador del Este",
                Phone = "+8091234567",
                UserType = "Premium",
                Money = 27000
            };

            var userService = new UserService(repository.Object);

            // Act
            var result = await userService.Create(userDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Messages);
        }

        [Fact]
        public async void UserService_CreateUser_ShouldReturnDuplicatedError()
        {
            // Arrange
            var repository = new Mock<IUserRepository>();

            var users = new List<User>(){
                new User {
                    Name = "JoseLuis",
                    Email = "josel@gmail.com",
                    Address = "C/ Mirador del Este",
                    Phone = "+5491154762312",
                    UserType = UserType.Normal,
                    Money = 1234
                }
            };

            repository.Setup(x => x.GetAllUser()).ReturnsAsync(users);
            var userDto = new UserDto
            {
                Name = "JoseLuis",
                Email = "josel@gmail.com",
                Address = "C/ Mirador del Este",
                Phone = "+8091234567",
                UserType = "Premium",
                Money = 27000
            };

            var userService = new UserService(repository.Object);

            // Act
            var result = await userService.Create(userDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("The User is duplicated", result.Messages);
        }
    }
}
