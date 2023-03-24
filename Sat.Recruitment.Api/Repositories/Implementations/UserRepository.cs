using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repositories.interfaces;

namespace Sat.Recruitment.Api.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly string _filePath;

        public UserRepository(IConfiguration iConfiguration)
        {
            this._iConfiguration = iConfiguration;
            _filePath = Directory.GetCurrentDirectory() + _iConfiguration["UserFilePath"];
        }


        public async Task<List<User>> GetAllUser()
        {
            List<User> _users = new List<User>();
            var reader = Utils.Utilitiess.ReadUsersFromFile(_filePath);

            while (reader.Peek() >= 0)
            {

                var line = await reader.ReadLineAsync();
                Enum.TryParse(line.Split(',')[4].ToString(), true, out UserType userType);
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = Utils.Utilitiess.NormalizeEmail(line.Split(',')[1].ToString()),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = userType,
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };

                _users.Add(user);
            }
            reader.Close();

            return _users;

        }

        /// <summary>
        /// Add user to file
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUser(User user){
            string[] userArr = {user.Name, user.Email, user.Phone, user.Address, user.UserType.ToString(), user.Money.ToString()};
            await File.AppendAllTextAsync(_filePath, string.Join(',', userArr) + Environment.NewLine);
        }
    }
}