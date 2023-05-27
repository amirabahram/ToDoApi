using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Entities.Interfaces;
using ToDoApi.Entities.Models;
using ToDoApi.Entities.ViewModels;
using ToDoApi.Services.Interfaces;

namespace ToDoApi.Services.Implementations
{
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            this._taskRepository = taskRepository;
        }

        public bool DeleteTask(UserTaskViewModel task)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserTaskViewModel>> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserTaskViewModel>> GetTasksByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertTask(UserTaskViewModel task)
        {
            throw new NotImplementedException();
        }



        public bool UpdateTask(UserTaskViewModel task)
        {
            throw new NotImplementedException();
        }
    }
}
