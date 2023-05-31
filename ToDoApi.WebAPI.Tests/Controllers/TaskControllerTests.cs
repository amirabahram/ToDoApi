

using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using ToDoApi.Domain.ViewModels;
using ToDoApi.Entities.ViewModels;
using ToDoApi.Services.Interfaces;
using ToDoApi.WebAPI.Controllers;
using Xunit;

namespace ToDoApi.WebAPI.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly TaskController _sut;  // system under test

        public TaskControllerTests()
        {
            _fixture = new Fixture();
            _taskServiceMock = _fixture.Freeze<Mock<ITaskService>>();
            _userServiceMock = _fixture.Freeze<Mock<IUserService>>();
            _sut = new  TaskController(_taskServiceMock.Object,_userServiceMock.Object);
        }
        [Fact]
        public async Task CreateTask_ShouldReturnOkResult_WhenTaskInserted()
        {

            var stringMocks = _fixture.Create<string>();       /// creates random string
            _userServiceMock.Setup(x => x.GetUserId()).ReturnsAsync(stringMocks);
            var newModel = new InsertTaskViewModel()
            {
                Description = "TaskInsertingTest",
            };
            var boolMoks = _fixture.Create<bool>();
            _taskServiceMock.Setup(x => x.InsertTask(newModel, stringMocks)).ReturnsAsync(boolMoks);


            //Act

            var result = await _sut.CreateTask(newModel);


            //Asset
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be(newModel.Description);
            
        }
        [Fact]
        public async Task CreateTask_ShouldReturnBadRequest_WhenTaskIsNull()
        {

            var stringMocks = _fixture.Create<string>();       /// creates random string
            _userServiceMock.Setup(x => x.GetUserId()).ReturnsAsync(stringMocks);
            InsertTaskViewModel newModel = null; 
            var boolMoks = false;
            _taskServiceMock.Setup(x => x.InsertTask(newModel, stringMocks)).ReturnsAsync(boolMoks);


            //Act

            var result = await _sut.CreateTask(newModel);


            //Asset
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestResult>();



        }
        [Fact]
        public async Task GetAllTasks_ShouldReturnIEnumreable_WhenCalls()
        {
            var getAllTasksMock = _fixture.Create<IEnumerable<UserTaskViewModel>>();
            _taskServiceMock.Setup(x=>x.GetAllTasks()).ReturnsAsync(getAllTasksMock);
            var result = await _sut.GetAllTasks();
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<UserTaskViewModel>>();
        }

        [Fact]
        public async Task FilterTasksByEmail_ShouldReturnIEnumreable_WhenEmailExists()
        {
            var getTaskMock = _fixture.Create<IEnumerable<UserTaskViewModel>>();
            var email = _fixture.Create<string>();
            _taskServiceMock.Setup(x=>x.GetTasksByUserEmail(email)).ReturnsAsync(getTaskMock);
            var result = await _sut.FilterTasksByEmail(email);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<UserTaskViewModel>>();
            
        }
        [Fact]
        public async Task FilterTasksByEamil_ShouldReturnEmptyIEnumreble_WhenEmailDoesntExists()
        {
            var getTaskMock = _fixture.Create<IEnumerable<UserTaskViewModel>>();
            string email = _fixture.Create<string>();
            _taskServiceMock.Setup(x => x.GetTasksByUserEmail(email)).ThrowsAsync(new Exception());
            var result = await _sut.FilterTasksByEmail(email);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<UserTaskViewModel>>();
        }
        [Fact]
        public async Task UpdateTaskById_ShouldReturnViewModel_WhenTaskExists()
        {
            var viewModel = _fixture.Create<UpdateTaskViewModel>();
            var inputModel = _fixture.Create<UpdateTaskViewModel>();
            _taskServiceMock.Setup(x=>x.UpdateTask(inputModel)).ReturnsAsync(viewModel);
            var result = await _sut.UpdateTaskById(inputModel);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<UpdateTaskViewModel>>();

        }
        [Fact]
        public async Task UpdateTaskById_ShouldReturnViewModel_WhenTaskDoesntExists()
        {
            UpdateTaskViewModel viewModel = null;
            var inputModel = _fixture.Create<UpdateTaskViewModel>();
            _taskServiceMock.Setup(x => x.UpdateTask(inputModel)).ReturnsAsync(viewModel);
            var result = await _sut.UpdateTaskById(inputModel);
            Assert.IsType<NotFoundObjectResult>(result.Result);
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.Equal("Task Not Found!", notFoundResult.Value);


        }
        [Fact]
        public async Task DeleteTaskById_ShouldReturnOkResult_WhenTaskExsists()
        {
            var id = _fixture.Create<int>();
           
            _taskServiceMock.Setup(x => x.DeleteTask(id)).ReturnsAsync(true);
            var result =await _sut.DeleteTaskById(id);
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task DeleteTaskById_ShouldReturnNotFound_WhenTaskDoesntExist()
        {
            var id = _fixture.Create<int>();

            _taskServiceMock.Setup(x => x.DeleteTask(id)).ReturnsAsync(false);
            var result = await _sut.DeleteTaskById(id);
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}