﻿using Domain.ViewModels;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Entities.ViewModels;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public UserService(UserManager<IdentityUser> userManager,
             IConfiguration _configuration,
            IHttpContextAccessor httpContext)
        {
            this._userManager = userManager;
            //this._roleManager = roleManager;
            this._configuration = _configuration;
            this._httpContext = httpContext;
        }

        public async Task<bool> EmailExists(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if(result != null)
            {
                return true;
            }
            return false;
        }

        public async Task<ResponseViewModel> GenerateToken(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q=>new Claim(ClaimTypes.Role, q)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                
               
            }
            .Union(userClaims)
            .Union(roleClaims);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JWT:Duration"])),
                signingCredentials: credentials
                );
            var response = new ResponseViewModel()
            {
                Email = user.Email,
                TokenString = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id
            };
            return response;
            

                
        }
        
        public async Task<string> GetUserId()
        {
            return   _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            
        }

        public async Task<bool> LoginUser(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return false;
            return await _userManager.CheckPasswordAsync(user, model.Password);



        }

        public async Task<IdentityResult> RegisterUser(RegisterViewModel model)
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

        public async Task<bool> UpdateUser(UpdateUserViewModel model )
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token,model.Password);
            if (result.Succeeded)
            {
                user.UserName = model.UserName;
                await _userManager.UpdateAsync(user);
                return true;
            }
            else
            {
                return false;
            }
            

        }
    }
}
