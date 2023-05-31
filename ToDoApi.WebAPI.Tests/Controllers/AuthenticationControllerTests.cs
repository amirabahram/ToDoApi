using Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services.Interfaces;
using ToDoApi.Controllers;
using ToDoApi.Entities.ViewModels;

namespace ToDoApi.WebAPI.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuthenticationController _sut;
        public AuthenticationControllerTests()
        {
            _fixture = new Fixture();
            _userServiceMock = _fixture.Freeze<Mock<IUserService>>();
            _sut = new AuthenticationController(_userServiceMock.Object);
        }
        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenModelStateIsNotValid()
        {
            var model = _fixture.Create<RegisterViewModel>();

            _sut.ModelState.AddModelError("Email", "Email is Required!!");
            var result = await _sut.Register(model);
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestResult>();

        }
        [Fact]
        public async Task Register_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
            var model = _fixture.Create<RegisterViewModel>();

            _userServiceMock.Setup(x => x.EmailExists(model.Email)).ReturnsAsync(true);

            var result = await _sut.Register(model);
            result.Should().NotBeNull();
            result.Should().BeOfType<ConflictObjectResult>();

        }
        [Fact]
        public async Task Register_ShouldReturnBadRequest_RegisterFieldsDoesNotSatisfing()
        {
            var model = _fixture.Create<RegisterViewModel>();

            _userServiceMock.Setup(x => x.EmailExists(model.Email)).ReturnsAsync(false);

            var expectedErrors = new List<IdentityError>
                 {
                    new IdentityError { Code = "Error1", Description = "Error description 1" },
                    new IdentityError { Code = "Error2", Description = "Error description 2" },
                };
            _userServiceMock.Setup(x => x.RegisterUser(model)).ReturnsAsync(IdentityResult.Failed(expectedErrors.ToArray()));
            var result = await _sut.Register(model);
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();



        }
        [Fact]
        public async Task Register_ShouldReturnOkResult_WhenRegisterDone()
        {
            var model = _fixture.Create<RegisterViewModel>();

            //_userServiceMock.Setup(x => x.EmailExists(model.Email)).ReturnsAsync(false);

            _userServiceMock.Setup(x => x.RegisterUser(model)).ReturnsAsync(IdentityResult.Success);
            var result = await _sut.Register(model);
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();



        }
        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenModelStateIsNotValid()
        {
            var model = _fixture.Create<LoginViewModel>();
            
            _sut.ModelState.AddModelError("Email", "Email is Required!!");

            _userServiceMock.Setup(x => x.LoginUser(model)).ReturnsAsync(true);
            var response = _fixture.Create<ResponseViewModel>();
            _userServiceMock.Setup(x => x.GenerateToken(model)).ReturnsAsync(response);
            var result = await _sut.Login(model);
            Assert.IsType<BadRequestResult>(result.Result);

        }
        [Fact]
        public async Task Login_ShouldReturnUnAuthorized_WhenUserNotFound()
        {
            var model = _fixture.Create<LoginViewModel>();

            _userServiceMock.Setup(x => x.LoginUser(model)).ReturnsAsync(false);
            var response = _fixture.Create<ResponseViewModel>();
            _userServiceMock.Setup(x => x.GenerateToken(model)).ReturnsAsync(response);
            var result = await _sut.Login(model);
            Assert.IsType<UnauthorizedObjectResult>(result.Result);

        }
        [Fact]
        public async Task Login_ShouldReturnResponseViewModel_WhenUserLoggedIn()
        {
            var model = _fixture.Create<LoginViewModel>();

            _userServiceMock.Setup(x => x.LoginUser(model)).ReturnsAsync(true);
            var response = _fixture.Create<ResponseViewModel>();
            _userServiceMock.Setup(x => x.GenerateToken(model)).ReturnsAsync(response);
            var result = await _sut.Login(model);
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<ResponseViewModel>>();

        }
    }
}