using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Domain.ViewModels;
using ToDoApi.Entities.Models;
using ToDoApi.Entities.ViewModels;

namespace ToDoApi.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<UserTaskViewModel>> GetAllTasks();
        Task<IEnumerable<UserTaskViewModel>> GetTasksByUserEmail(string email);
        Task<string> GetUserIdByTaskId(int taskId);
        Task<bool> InsertTask(InsertTaskViewModel task,string userId);
        Task<UpdateTaskViewModel> UpdateTask(UpdateTaskViewModel task);
        Task<bool> DeleteTask(int taskId);

    }
}
