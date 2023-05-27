using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Entities.Models;

namespace ToDoApi.Entities.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllTasks();
        Task<IEnumerable<UserTask>> GetTasksByUser(string userId);
        Task InsertTask(UserTask task);
        void UpdateTask(UserTask task);
        void DeleteTask(UserTask task);
        Task Save();
    }
}
