using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Entities.Models;
using ToDoApi.Entities.ViewModels;

namespace ToDoApi.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<UserTaskViewModel>> GetAllTasks();
        Task<IEnumerable<UserTaskViewModel>> GetTasksByUser(string userId);
        Task<bool> InsertTask(UserTaskViewModel task);
        bool UpdateTask(UserTaskViewModel task);
        bool DeleteTask(UserTaskViewModel task);

    }
}
