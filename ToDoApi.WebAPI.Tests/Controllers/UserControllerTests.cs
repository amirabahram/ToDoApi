

using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using ToDoApi.Domain.ViewModels;
using ToDoApi.Entities.ViewModels;
using ToDoApi.Services.Interfaces;
using ToDoApi.WebAPI.Controllers;
using WebAPI.Controllers;
using Xunit;

namespace ToDoApi.WebAPI.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _sut;  // system under test

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _userServiceMock = _fixture.Freeze<Mock<IUserService>>();
            _sut = new  UserController(_userServiceMock.Object);
        }
        [Fact]
        public async Task UpdateUser_ShouldReturnBadRequest_WhenModelStateIsnotValid()
        {
            //Arrange

            UpdateUserViewModel model = _fixture.Create<UpdateUserViewModel>();
            _userServiceMock.Setup(x=>x.UpdateUser(model)).ReturnsAsync(true);
            _sut.ModelState.AddModelError("Email", "Email is not valid");

            //Act

            var result = await _sut.UpdateUser(model);


            //Asset
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestResult>();

            _userServiceMock.Verify(x=>x.UpdateUser(model),Times.Never());
            
        }
        [Fact]
        public async Task UpdateUser_ShouldReturnOkResult_WhenUserUpdated()
        {
            //Arrange

            UpdateUserViewModel model = _fixture.Create<UpdateUserViewModel>();
            _userServiceMock.Setup(x => x.UpdateUser(model)).ReturnsAsync(true);


            //Act

            var result = await _sut.UpdateUser(model);


            //Asset
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            

            _userServiceMock.Verify(x => x.UpdateUser(model), Times.Once);
        }
        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserUpdatFailed()
        {
            //Arrange

            UpdateUserViewModel model = _fixture.Create<UpdateUserViewModel>();
            _userServiceMock.Setup(x => x.UpdateUser(model)).ReturnsAsync(false);


            //Act

            var result = await _sut.UpdateUser(model);


            //Asset
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();


            _userServiceMock.Verify(x => x.UpdateUser(model), Times.Once);
        }

    }
}