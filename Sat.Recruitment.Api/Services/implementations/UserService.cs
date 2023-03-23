using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repositories.interfaces;
using Sat.Recruitment.Api.Services.interfaces;

namespace Sat.Recruitment.Api.Services.implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _iUserRepository;
        public UserService(IUserRepository iUserRepository)
        {
            this._iUserRepository = iUserRepository;
        }


        /// <summary>
        /// Maps UserDto to User
        /// <param name="userDto"></param>
        /// </summary>
        private User MapDtoToUser(UserDto userDto)
        {

            var parsedEnum = Enum.TryParse(userDto.UserType, true, out UserType userType);
            if (!parsedEnum)
            {
                throw new ValidationException($"UserType {userDto.UserType} is invalid, valid types of user: Normal, SuperUser or Premium");
            }

            return new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Address = userDto.Address,
                Phone = userDto.Phone,
                Money = userDto.Money,
                UserType = userType
            };
        }

        private void CalculateMoneyByUserType(User user)
        {
            switch (user.UserType)
            {

                case UserType.Normal:
                    if (user.Money > 100)
                    {
                        SetMoneyPercentage(user, 0.12);
                    }
                    else if (user.Money < 100 && user.Money > 10)
                    {
                        SetMoneyPercentage(user, 0.8);
                    }
                    break;
                case UserType.SuperUser:
                    if (user.Money > 100)
                    {
                        SetMoneyPercentage(user, 0.20);
                    }
                    break;
                case UserType.Premium:
                    if (user.Money > 100)
                    {
                        SetMoneyPercentage(user, 2);
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets the percentage to user's money
        /// </summary>
        /// <param name="user"></param>
        /// <param name="moneyPercentage"></param>
        private void SetMoneyPercentage(User user, double moneyPercentage)
        {
            var gif = user.Money * Convert.ToDecimal(moneyPercentage);
            user.Money += gif;
        }


        /// <summary>
        /// Check for duplicated users
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        private async Task<Result> CheckDuplicatedUsers(User newUser){
            var users = await this._iUserRepository.GetAllUser();
            string errors = string.Empty;

             var isDuplicated = false;
            if (users.Exists((user) => newUser.Email.Equals(user.Email) || newUser.Phone.Equals(user.Phone)))
            {
                isDuplicated = true;
            } else if (users.Exists((user) => newUser.Name.Equals(user.Name) && newUser.Address.Equals(user.Address)))
            {
                isDuplicated = true;
            }

            if (!isDuplicated)
            {
                return new Result(){
                    IsSuccess = true,
                    Messages = "User Created"
                };

            } else {
                return new Result(){
                    IsSuccess = false,
                    Messages = "The User is duplicated"
                };
            }
        }

        /// <summary>
        /// Create user and returns the result
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<Result> Create(UserDto userDto){

            var user = MapDtoToUser(userDto);

            CalculateMoneyByUserType(user);

            var result = await CheckDuplicatedUsers(user);

            if (!result.IsSuccess)
            {
                return result;
            }

            await this._iUserRepository.AddUser(user);

            return result;
        }
    }
}