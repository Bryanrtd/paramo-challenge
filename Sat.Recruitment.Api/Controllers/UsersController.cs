using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Services.interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _iUserService;
        public UsersController(IUserService iUserService)
        {
            this._iUserService = iUserService;
        }

        [HttpPost]
        public async Task<Result> CreateUser([Required][FromBody]UserDto user)
        {
            try
            {
                return await this._iUserService.Create(user);
            }
            catch (System.Exception ex)
            {
                return new Result() {
                    IsSuccess = false,
                    Messages = ex.Message
                };
            }
        }

    }

}
