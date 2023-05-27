using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApi.Entities.ViewModels;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserService _userService;




        public AuthenticationController(IUserService userService)
        {
            this._userService = userService;



        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
        [HttpPost]
        public async Task<ActionResult<ResponseViewModel>> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var validateResult = await _userService.LoginUser(model);
            if (!validateResult)
            {
                return Unauthorized(model);
            }
            var response = await  _userService.GenerateToken(model);
            

            return response;
        }

        
    }
}
