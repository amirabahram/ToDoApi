using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ToDoApi.Domain.Interfaces;
using ToDoApi.Domain.ViewModels;
using ToDoApi.Entities.Interfaces;
using ToDoApi.Entities.Models;
using ToDoApi.Entities.ViewModels;
using ToDoApi.Services.Implementations;

namespace ToDoApi.Application.Tests.Implementations
{
    
    public class TaskServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly TaskService _sut;


        public TaskServiceTests()
        {
            _fixture = new Fixture();
            _taskRepositoryMock = _fixture.Freeze<Mock<ITaskRepository>>();
            _userRepositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
            _userManagerMock = MockHelpers.MockUserManager<IdentityUser>();
            _sut = new TaskService(_taskRepositoryMock.Object, _userRepositoryMock.Object,_userManagerMock.Object);

        }



        [Fact]
        public async Task GetAllTasks_ShouldReturnIEnemerableOfViewModel_WhenCalled()
        {
            var list = _fixture.Create<List<UserTask>>();
            var id = _fixture.Create<string>();
            var identityUser = _fixture.Create<IdentityUser>();
           _taskRepositoryMock.Setup(x=>x.GetAllTasks()).ReturnsAsync(list);
           _userRepositoryMock.Setup(x=>x.GetUserById(id)).ReturnsAsync(identityUser);
            var result = await _sut.GetAllTasks();
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<UserTaskViewModel>>();
            _taskRepositoryMock.Verify(x=>x.GetAllTasks(), Times.Once);

        }
        [Fact]
        public async Task InsertTask_ShouldReturnfalse_WhenTaskIsNull()
        {
            InsertTaskViewModel task = null;
            var id = _fixture.Create<string>();
            var userTask = _fixture.Create<UserTask>();

            _taskRepositoryMock.Setup(x => x.InsertTask(userTask));
            _taskRepositoryMock.Setup(x => x.Save());
            var result = await _sut.InsertTask(task, id);
            Assert.False(result);
            _taskRepositoryMock.Verify(repo => repo.InsertTask(It.IsAny<UserTask>()), Times.Never);
            _taskRepositoryMock.Verify(repo => repo.Save(), Times.Never);


        }
        [Fact]
        public async Task InsertTask_ShouldReturnfalse_WhenIdIsNull()
        {
            InsertTaskViewModel task = _fixture.Create<InsertTaskViewModel>();

            string id = null;
            var userTask = _fixture.Create<UserTask>();

            _taskRepositoryMock.Setup(x => x.InsertTask(userTask));
            _taskRepositoryMock.Setup(x => x.Save());
            var result = await _sut.InsertTask(task, id);
            Assert.False(result);
            _taskRepositoryMock.Verify(repo => repo.InsertTask(It.IsAny<UserTask>()), Times.Never);
            _taskRepositoryMock.Verify(repo => repo.Save(), Times.Never);


        }
        [Fact]
        public async Task InsertTask_ShouldReturnTrue_WhenTaskInserted()
        {
            InsertTaskViewModel task = _fixture.Create<InsertTaskViewModel>();
            var id = _fixture.Create<string>();
            var userTask = _fixture.Create<UserTask>();

            _taskRepositoryMock.Setup(x => x.InsertTask(userTask));
            _taskRepositoryMock.Setup(x => x.Save());
            var result = await _sut.InsertTask(task, id);
            Assert.True(result);


        }
        [Fact]
        public async Task UpdateTask_ShouldReturnNull_WhenOldTaskDoesntExist()
        {
            var task = _fixture.Create<UpdateTaskViewModel>();
            UserTask userTask = null;
            _taskRepositoryMock.Setup(x => x.GetTaskById(task.TaskId)).ReturnsAsync(userTask);
            _taskRepositoryMock.Setup(x => x.UpdateTask(userTask));
            _taskRepositoryMock.Setup(x=>x.Save());

            var result = await _sut.UpdateTask(task);
            Assert.Null(result);
            _taskRepositoryMock.Verify(repo => repo.UpdateTask(userTask), Times.Never);
            _taskRepositoryMock.Verify(repo => repo.Save(), Times.Never);

        }
        [Fact]
        public async Task UpdateTask_ShouldReturnUpdateTaskViewModel_WhenTaskUpdated()
        {
            var task = _fixture.Create<UpdateTaskViewModel>();
            UserTask userTask = _fixture.Create<UserTask>();
            _taskRepositoryMock.Setup(x => x.GetTaskById(task.TaskId)).ReturnsAsync(userTask);
            _taskRepositoryMock.Setup(x => x.UpdateTask(userTask));
            _taskRepositoryMock.Setup(x => x.Save());

            var result = await _sut.UpdateTask(task);
            result.Should().BeAssignableTo<UpdateTaskViewModel>();
            _taskRepositoryMock.Verify(repo => repo.UpdateTask(userTask), Times.Once);
            _taskRepositoryMock.Verify(repo => repo.Save(), Times.Once);

        }
        [Fact]
        public async Task DeleteTask_ShouldReturnFalse_WhenTaskIdEqualsZero()
        {
            int taskId = 0;
            var userTask = _fixture.Create<UserTask>();
            _taskRepositoryMock.Setup(x => x.GetTaskById(taskId)).ReturnsAsync(userTask);
            var result = await _sut.DeleteTask(taskId);
            Assert.False(result);
            _taskRepositoryMock.Verify(r=>r.DeleteTask(userTask), Times.Never);
            _taskRepositoryMock.Verify(r=>r.Save(), Times.Never);
        }
        [Fact]
        public async Task DeleteTask_ShouldReturnFalse_WhenTaskDoesntExists()
        {
            int taskId = _fixture.Create<int>();
            UserTask userTask = null;
            _taskRepositoryMock.Setup(x => x.GetTaskById(taskId)).ReturnsAsync(userTask);
            var result = await _sut.DeleteTask(taskId);
            Assert.False(result);
            _taskRepositoryMock.Verify(r => r.DeleteTask(userTask), Times.Never);
            _taskRepositoryMock.Verify(r => r.Save(), Times.Never);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnTrue_WhenTaskDeleted()
        {
            int taskId = _fixture.Create<int>();
            UserTask userTask = _fixture.Create<UserTask>();
            _taskRepositoryMock.Setup(x => x.GetTaskById(taskId)).ReturnsAsync(userTask);
            var result = await _sut.DeleteTask(taskId);
            Assert.True(result);
            _taskRepositoryMock.Verify(r => r.DeleteTask(userTask), Times.Once);
            _taskRepositoryMock.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public async Task GetTasksByUserEmail_ShouldReturnIEnumerableOfViewModel_WhenCalls()
        {
            var email = _fixture.Create<string>();
            IdentityUser user = _fixture.Create<IdentityUser>();
            var tasks = _fixture.Create<IEnumerable<UserTask>>();
            _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
            _taskRepositoryMock.Setup(x => x.GetTasksByUser(user.Id)).ReturnsAsync(tasks);
            var result = await _sut.GetTasksByUserEmail(email);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<UserTaskViewModel>>();
            _taskRepositoryMock.Verify(r=>r.GetTasksByUser(user.Id),Times.Once);
            _userManagerMock.Verify(r=>r.FindByEmailAsync(email),Times.Once);
        }
    }
}