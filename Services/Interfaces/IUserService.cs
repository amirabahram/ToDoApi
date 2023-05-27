using Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Entities.ViewModels;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> EmailExists(string email);
        Task<IdentityResult> RegisterUser(RegisterViewModel model);
        Task<bool> LoginUser(LoginViewModel model);
        Task<ResponseViewModel> GenerateToken(LoginViewModel model);
        Task<string> GetUserId();
    }
}
