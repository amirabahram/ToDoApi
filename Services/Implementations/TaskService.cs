using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Domain.Interfaces;
using ToDoApi.Domain.ViewModels;
using ToDoApi.Entities.Interfaces;
using ToDoApi.Entities.Models;
using ToDoApi.Entities.ViewModels;
using ToDoApi.Services.Interfaces;

namespace ToDoApi.Services.Implementations
{
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository,
            UserManager<IdentityUser> userManager)
        {
            this._taskRepository = taskRepository;
            this._userRepository = userRepository;
            this._userManager = userManager;
        }



        public async Task<IEnumerable<UserTaskViewModel>> GetAllTasks()
        {
            var list = new List<UserTaskViewModel>();
            var result = await _taskRepository.GetAllTasks();
            foreach(var task in result)
            {
                var user = await _userRepository.GetUserById(task.UserId);
                var taskViewModel = new UserTaskViewModel()
                {
                    Description = task.Description,
                    UserName = user.UserName,
                    TaskId = task.Id

                };
                list.Add(taskViewModel);
            }
            return list;
        }





        public async Task<bool> InsertTask(InsertTaskViewModel task, string userId)
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

        public  async Task<UpdateTaskViewModel> UpdateTask(UpdateTaskViewModel task)
        {
            
            var oldTask  =await _taskRepository.GetTaskById(task.TaskId);
            oldTask.Description = task.Description;
            _taskRepository.UpdateTask(oldTask);
            _taskRepository.Save();
            var newTask = await _taskRepository.GetTaskById(task.TaskId);
            var taskViewModel = new UpdateTaskViewModel()
            {
                TaskId = task.TaskId,
                Description = task.Description
            };
            return taskViewModel;
        }



        public async Task<bool> DeleteTask(int taskId)
        {
            if (taskId == 0) return false;
            var task = await _taskRepository.GetTaskById(taskId);
            _taskRepository.DeleteTask(task);
            _taskRepository.Save();
            return true;
        }

        public async Task<IEnumerable<UserTaskViewModel>> GetTasksByUserEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var list = new List<UserTaskViewModel>();
            var tasks = await _taskRepository.GetTasksByUser(user.Id);
            
            foreach (var item in tasks)
            {
                var listItem = new UserTaskViewModel()
                {
                    Description = item.Description,
                    UserName = item.User.UserName,
                    TaskId = item.Id
                };
                list.Add(listItem);
            }
            return list;
        }
    }
}
