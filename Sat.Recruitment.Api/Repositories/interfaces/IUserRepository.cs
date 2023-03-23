using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Model;

namespace Sat.Recruitment.Api.Repositories.interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUser();
        Task AddUser(User user);
    }
}