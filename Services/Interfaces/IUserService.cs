using Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> EmailExists(string email);
        Task<IdentityResult> RegisterUser(UserViewModel model);
    }
}
