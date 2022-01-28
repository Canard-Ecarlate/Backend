using AutoMapper;
using DuckCity.Api.Controllers;
using DuckCity.Api.DTO.Authentication;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Api
{
    public class AuthenticationControllerUnitTests
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<HttpContext> _mockHttpContext = new();
        private readonly AuthenticationController _authenticationController;

        public AuthenticationControllerUnitTests()
        {
            _authenticationController =
                new AuthenticationController(_mockAuthenticationService.Object, _mockMapper.Object)
                {
                    ControllerContext =
                    {
                        HttpContext = _mockHttpContext.Object
                    }
                };
        }

        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.String)]
        public void LoginTest(string name, string password)
        {
            IdentifierDto identifierDto = new() {Name = name, Password = password};
            User user = new() {Name = name, Password = password};

            _mockAuthenticationService.Setup(mock => mock.Login(name, password)).Returns(user);
            _mockMapper.Setup(mock => mock.Map<UserWithTokenDto>(user))
                .Returns(new UserWithTokenDto {Name = name});

            ActionResult<UserWithTokenDto> actionResult = _authenticationController.Login(identifierDto);
            OkObjectResult? result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            UserWithTokenDto res = (UserWithTokenDto) result?.Value!;
            Assert.Equal(name, res.Name);
            Assert.NotEmpty(res.Token!);
        }
    }
}