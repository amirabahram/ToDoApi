using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Diagnostics.Eventing.Reader;
using ToDoApi.Entities.ViewModels;
using ToDoApi.Services.Interfaces;


namespace ToDoApi.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        public TaskController(ITaskService taskService, IUserService userService)
        {
            this._taskService = taskService;
            this._userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTask(UserTaskViewModel model)
        {
            var id = await _userService.GetUserId();
            
            var res = await _taskService.InsertTask(model,id);
            if (res)
            {
                return Ok(model.Description);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IEnumerable<UserTaskViewModel>> GetAllTasks()
        {
            var result = await _taskService.GetAllTasks();
            return result;
        }

    }
}
