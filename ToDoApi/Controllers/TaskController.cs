using Domain.ViewModels;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Interfaces;
using System.Diagnostics.Eventing.Reader;

using ToDoApi.Domain.ViewModels;
using ToDoApi.Entities.ViewModels;
using ToDoApi.Services.Interfaces;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;

namespace ToDoApi.WebAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]/[action]")]
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
        public async Task<IActionResult> CreateTask(InsertTaskViewModel model)
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

        [HttpGet("{Email}")]
        public async Task<IEnumerable<UserTaskViewModel>> FilterTasksByEmail(string Email)
        {

            try
            {
                var result = await _taskService.GetTasksByUserEmail(Email);
                return result;
            }
            catch (Exception ex)
            {

                return Enumerable.Empty<UserTaskViewModel>();
            }
        }
        [HttpPut]
        [Authorize("MustBeCreatorOfTask")]
        public async Task<ActionResult<UpdateTaskViewModel>> UpdateTaskById( UpdateTaskViewModel model)
        {

            var result = await _taskService.UpdateTask(model);
            if (result != null)
            {
                return result;
            }
            else
            {
                return NotFound("Task Not Found!");
            }


        }
        [HttpDelete]
        [Authorize("MustBeCreatorOfTask")]
        public async Task<IActionResult> DeleteTaskById(DeleteTaskViewModel model)
        {
            var result = await _taskService.DeleteTask(model.Id);
            if (result) { return Ok($"The Task By id of {model.Id} deleted successfuly!"); }
            else { return NotFound("Task Not Found!") ; }
            
         }
    }
}
