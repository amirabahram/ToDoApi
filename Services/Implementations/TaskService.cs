using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

        public async Task<IEnumerable<UserTaskViewModel>> GetAllTasks()
        {
            var list = new List<UserTaskViewModel>();
            var result = await _taskRepository.GetAllTasks();
            foreach(var task in result)
            {
                var taskViewModel = new UserTaskViewModel()
                {
                    Description = task.Description

                };
                list.Add(taskViewModel);
            }
            return list;
        }

        public Task<IEnumerable<UserTaskViewModel>> GetTasksByUser(string userId)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> InsertTask(UserTaskViewModel task, string userId)
        {
            if (task == null) return false;
            var entityTask = new UserTask()
            {
                UserId = userId,
                Description = task.Description,
                CreatedDate = DateTime.Now
            };
            await _taskRepository.InsertTask(entityTask);
             _taskRepository.Save();
            return true;
        }

        public bool UpdateTask(UserTaskViewModel task)
        {
            throw new NotImplementedException();
        }
    }
}
