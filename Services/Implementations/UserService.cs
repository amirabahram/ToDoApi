using Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration _configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = _configuration;
        }

        public async Task<bool> EmailExists(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if(result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<IdentityResult> RegisterUser(UserViewModel model)
        {
            IdentityUser user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded) await _userManager.AddToRoleAsync(user,"User");
            return result;
        }

        
    }
}
