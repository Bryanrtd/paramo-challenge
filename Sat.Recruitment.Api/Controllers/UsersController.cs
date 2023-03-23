using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        [HttpPost("/create-user")]
        public async Task<Result> CreateUser(UserDto user)
        {
            return await this._iUserService.Create(user);
        }

    }

}
