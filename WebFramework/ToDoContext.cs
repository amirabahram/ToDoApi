

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
            builder.Entity<UserTask>().HasKey(x => x.Id);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name="User",ConcurrencyStamp="1",NormalizedName="User"},
                new IdentityRole() { Name="Admin",ConcurrencyStamp="1",NormalizedName="Admin"}
                   
                );
        }

        public DbSet<UserTask> Tasks { get; set; }
    }
}