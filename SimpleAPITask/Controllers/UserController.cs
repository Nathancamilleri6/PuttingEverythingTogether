using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Domain;
using SimpleAPITask.DTOs;
using SimpleAPITask.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAPITask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUsers _IUser;
        private readonly IUserServices _IUserService;

        public UserController(IUsers IUser, IUserServices IUserService)
        {
            _IUser = IUser;
            _IUserService = IUserService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserOutputDTO>>> GetUsers()
        {
            return await Task.FromResult(_IUser.GetUserDetails());
        }

        public async Task<ActionResult<User>> GetUser([FromBody] UserInputDTO User)
        {
            return await Task.FromResult(_IUser.GetUser(User));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserInputDTO>> AddUser([FromBody] UserInputDTO User)
        {
            _IUser.AddUser(User);
            return await Task.FromResult(User);
        }

		[AllowAnonymous]
		[HttpPost]
		[Route("/api/users/authenticate")]
		public async Task<Token> Authenticate([FromBody] User User)
		{
			return await _IUserService.Authenticate(User);
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("/api/users/refresh")]
		public async Task<Token> Refresh([FromBody] Token Token)
		{
            return await _IUserService.Refresh(Token);
        }
	}
}
