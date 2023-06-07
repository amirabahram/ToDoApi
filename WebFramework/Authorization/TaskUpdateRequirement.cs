
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ToDoApi.Services.Interfaces;
using System.Text.Json;
using System.Security.Claims;

namespace Infrastructure.Authorization
{
    public class TaskUpdateRequirement : IAuthorizationRequirement
    {
    }

    public class TaskUpdateRequirementHandler : AuthorizationHandler<TaskUpdateRequirement>
    {
        private readonly ITaskService _taskService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaskUpdateRequirementHandler(ITaskService taskService,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskService = taskService;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override  Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TaskUpdateRequirement requirement)
        {

            if (!context.User.Identities.Any(c=>c.IsAuthenticated==true))//if user is logged in or not
            {
                context.Fail();
                return  Task.CompletedTask;
            }


            var bodyStr = "";
            var req = _httpContextAccessor.HttpContext.Request;

            // Allows using several time the stream in ASP.Net Core
            req.EnableBuffering();

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader
                      = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            // Rewind, so the core is not lost when it looks at the body for the request
            req.Body.Position = 0;
            // Do whatever works with bodyStr here

            var jsonDocument = JsonDocument.Parse(bodyStr);
            int id = jsonDocument.RootElement.GetProperty("id").GetInt32();


            var manipulatorId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string taskCreatorId =  _taskService.GetUserIdByTaskId(id).GetAwaiter().GetResult(); 
            if (taskCreatorId == manipulatorId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }






}
