using System.Threading.Tasks;
using Sat.Recruitment.Api.Model;

namespace Sat.Recruitment.Api.Services.interfaces
{
    public interface IUserService
    {
        Task<Result> Create(UserDto userDto);
    }
}