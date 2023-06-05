using Castle.Core.Configuration;
using Domain.ViewModels;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Implementations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToDoApi.Application.Tests.Implementations.Extensions;
using ToDoApi.Entities.ViewModels;

namespace ToDoApi.Application.Tests.Implementations
{
    public class UserServiceTests
    {


        private readonly IFixture _fixture;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerWithStoreMock;
        private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> _configurationMock;
        private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
        private readonly Mock<IUserStore<IdentityUser>> _storeMock;
        private readonly Mock<IdentityResult> _resultMock;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _fixture = new Fixture();
            _resultMock = new Mock<IdentityResult>();
            _userManagerMock = MockHelpers.MockUserManager<IdentityUser>();
            _storeMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerWithStoreMock = new Mock<UserManager<IdentityUser>>(_storeMock.Object);
            _configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "ConnectionStrings:default")]).Returns("mock value");
            _contextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();
            _storeMock = new Mock<IUserStore<IdentityUser>>();
            _sut = new UserService(_userManagerMock.Object, _configurationMock.Object,_contextAccessorMock.Object);
        }

        [Fact]
        public async Task EmailExists_ShouldReturnTrue_WhenEmailExists()
        {
            var email = _fixture.Create<string>();
            IdentityUser user = _fixture.Create<IdentityUser>();
            _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
            var result = await _sut.EmailExists(email);
            Assert.True(result);
            _userManagerMock.Verify(r=>r.FindByEmailAsync(email), Times.Once);
        }
        [Fact]
        public async Task EmailExists_ShouldReturnFalse_WhenEmailDoesntExists()
        {
            var email = _fixture.Create<string>();
            IdentityUser user = null;
            _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
            var result = await _sut.EmailExists(email);
            Assert.False(result);
            _userManagerMock.Verify(r => r.FindByEmailAsync(email), Times.Once);
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnResponseViewModel_WhenTokenGenerated()
        {
            var loginViewModel = _fixture.Create<LoginViewModel>();
            var identityUser = _fixture.Create<IdentityUser>();
            var roles = _fixture.Create<List<string>>();
            var claims = new List<Claim>();
            Thread.CurrentPrincipal = new TestPrincipal(new Claim("name", "JohnDoe"));
            _userManagerMock.Setup(x => x.FindByEmailAsync(loginViewModel.Email)).ReturnsAsync(identityUser);
            _userManagerMock.Setup(x => x.GetRolesAsync(identityUser)).ReturnsAsync(roles);
            _userManagerMock.Setup(x => x.GetClaimsAsync(identityUser)).ReturnsAsync(claims);

            _configurationMock.Setup(x => x["JWT:Key"]).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
            _configurationMock.Setup(x => x["JWT:Issuer"]).Returns("ToDoApi");
            _configurationMock.Setup(x => x["JWT:Audience"]).Returns("WebAPI");
            _configurationMock.Setup(c => c["JWT:Duration"]).Returns("60");




            
            var result = await _sut.GenerateToken(loginViewModel);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = result.TokenString;
            var token = tokenHandler.ReadJwtToken(tokenString);



            Assert.NotNull(result);
            Assert.Equal(identityUser.Email, result.Email);
            Assert.Equal(identityUser.Id, result.UserId);
            var audi = new List<string>();

            Assert.Equal("ToDoApi", token.Issuer);
            //Assert.Equal("WebAPI", token.Audiences);
            Assert.Equal(identityUser.Id, token.Subject);
            Assert.True(token.Claims.Any(claim => claim.Type == JwtRegisteredClaimNames.Email && claim.Value == identityUser.Email));
            Assert.True(token.Claims.Any(claim => claim.Type == JwtRegisteredClaimNames.Jti));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"));
            var tokenValidationParams = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidateAudience = true,
                ValidAudience = "WebAPI",
                ValidateIssuer = true,
                ValidIssuer = "ToDoApi"
            };

            tokenHandler.ValidateToken(tokenString, tokenValidationParams, out var validatedToken);

            Assert.NotNull(validatedToken);
        }

        [Fact]
        public async Task GetUserId_ShouldReturnString_WhenCalled()
        {
            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "12345") // Sample user ID
            }));
            httpContext.User = user;
            string id = "12345";
            Claim claim = new Claim(ClaimTypes.NameIdentifier, id);
            _contextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)).Returns(claim);

            var result = await _sut.GetUserId();

            result.Should().NotBeNull();
            Assert.Equal(id, result);
        }
        [Fact]
        public async Task LoginUser_ShouldReturnFalse_WhenUserNotFound()
        {
            var model= _fixture.Create<LoginViewModel>();
            IdentityUser user = null;
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x=>x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(true);

            var result = await _sut.LoginUser(model);

            result.Should().BeFalse();
            _userManagerMock.Verify(x=>x.CheckPasswordAsync(user,model.Password),Times.Never);
        }
        [Fact]
        public async Task LoginUser_ShouldReturnFalse_WhenPasswordDoesntMatch()
        {
            var model = _fixture.Create<LoginViewModel>();
            IdentityUser user = _fixture.Create<IdentityUser>();
            
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(false);

            var result = await _sut.LoginUser(model);

            result.Should().BeFalse();
            _userManagerMock.Verify(x => x.CheckPasswordAsync(user, model.Password), Times.Once);
        }
        [Fact]
        public async Task LoginUser_ShouldReturnTrue_WhenPasswordMatches()
        {
            var model = _fixture.Create<LoginViewModel>();
            IdentityUser user = _fixture.Create<IdentityUser>();

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(true);

            var result = await _sut.LoginUser(model);

            result.Should().BeTrue();
            _userManagerMock.Verify(x => x.CheckPasswordAsync(user, model.Password), Times.Once);
        }
        [Fact]
        public async Task UpdateUser_ShouldReturnFalse_WhenUserNotFound()
        {
            var model = _fixture.Create<UpdateUserViewModel>();
            IdentityUser user = null;
            string token = _fixture.Create<string>();

            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x=>x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);
            _userManagerMock.Setup(x=>x.ResetPasswordAsync(user,token,"123")).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.UpdateAsync(user));

            var result = await _sut.UpdateUser(model);

            result.Should().BeFalse();
            _userManagerMock.Verify(x=>x.FindByEmailAsync(model.Email),Times.Once);
            _userManagerMock.Verify(x=>x.GeneratePasswordResetTokenAsync(user), Times.Never);
            _userManagerMock.Verify(x=>x.ResetPasswordAsync(user, token, "123"), Times.Never);
            _userManagerMock.Verify(x=>x.UpdateAsync(user), Times.Never);
        }


    }
}