using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityUser> GetUserById(string userId);
        void UpdateUser(IdentityUser user);

    }
}
