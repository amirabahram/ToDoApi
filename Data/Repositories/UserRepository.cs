using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Domain.Interfaces;

namespace ToDoApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ToDoContext _db;
        public UserRepository(ToDoContext db)
        {
            this._db = db;
        }
        public async Task<IdentityUser> GetUserById(string userId)
        {
            return await _db.Users.FirstOrDefaultAsync(i => i.Id == userId);
        }
    }
}
