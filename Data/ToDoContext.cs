

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Entities.Models;

namespace Data
{
    public class ToDoContext : IdentityDbContext<IdentityUser> 
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name="User",ConcurrencyStamp="1",NormalizedName="User"}
                
                );
        }

        public DbSet<UserTask> Tasks { get; set; }
    }
}