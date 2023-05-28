using Microsoft.Extensions.DependencyInjection;
using Services.Implementations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Data.Repositories;
using ToDoApi.Domain.Interfaces;
using ToDoApi.Entities.Interfaces;
using ToDoApi.Services.Implementations;
using ToDoApi.Services.Interfaces;

namespace WebFramework
{
    public class DependencyContainers
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ITaskService,TaskService>();
            services.AddScoped<ITaskRepository,TaskRepository>();
            services.AddScoped<IUserRepository,UserRepository>();

        }
    }
}
