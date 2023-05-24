using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;


namespace ToDoApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserService _userService;
        public AuthenticationController(IUserService _userService)
        {
            this._userService = _userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var emailCheckResult = await _userService.EmailExists(model.Email);
            if(emailCheckResult ) return BadRequest();

            var createResult = await _userService.RegisterUser(model);

            if (createResult.Succeeded == false)
            {
                foreach(var item in createResult.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                    return BadRequest();
                }

            }
            return Ok();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //}

    }
}
