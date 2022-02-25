using DuckCity.Api.Controllers;
using DuckCity.Api.DTO.User;
using DuckCity.Application.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Api
{
    public class UserControllerUt
    {
        // Class to test
        private readonly UserController _userController;
        // Mock
        private readonly Mock<IUserService> _mockUserService = new();

        private readonly Mock<HttpContext> _mockHttpContext = new();
        
        private readonly Mock<HttpRequest> _mockHttpRequest = new();

        private readonly Mock<IHeaderDictionary> _mockHeaderDictionaryMock = new();

        // Constructor
        public UserControllerUt()
        {
            _userController =
                new UserController(_mockUserService.Object)
                {
                    ControllerContext = {HttpContext = _mockHttpContext.Object}
                };
            _mockHttpContext.Setup(context => context.Request).Returns(_mockHttpRequest.Object);
            _mockHttpRequest.Setup(request => request.Headers).Returns(_mockHeaderDictionaryMock.Object);
        }

        /**
     * Tests
     */
        [Theory]
        [InlineData(ConstantTest.UserId3,ConstantTest.Token3)]
        public void DeleteUserTest(string id, string token)
        {
            //Given
            _mockUserService.Setup(mock => mock.DeleteAccountUser(id));
            _mockHttpContext.Setup(a => a.Request.Headers["Authorization"])
                .Returns(token);
            
            //When
            ActionResult<string> actionResult = _userController.DeleteUser();
            OkObjectResult? result = actionResult.Result as OkObjectResult;
            
            //Then
            Assert.NotNull(result);
            string res = (string) result?.Value!;
            Assert.Equal("The account is deleted", res);
            
            //Verify
            _mockUserService.Verify(mock => mock.DeleteAccountUser(id), Times.Once);
        }
        
        [Theory]
        [InlineData(ConstantTest.UserId3, ConstantTest.String, ConstantTest.String,ConstantTest.String,ConstantTest.Token3)]
        public void ChangePasswordTest(string id, string actualPassword, string newPassword, string passwordConfirmation,string token)
        {
            //Given
            UserChangePassword userChangePassword = new() {ActualPassword = actualPassword,NewPassword = newPassword,PasswordConfirmation = passwordConfirmation};
            _mockUserService.Setup(mock => mock.ChangePasswordUser(id,actualPassword,newPassword,passwordConfirmation));
            _mockHttpContext.Setup(a => a.Request.Headers["Authorization"])
                .Returns(token);
            
            //When
            ActionResult<string> actionResult = _userController.ChangePassword(userChangePassword);
            OkObjectResult? result = actionResult.Result as OkObjectResult;

            //Then
            Assert.NotNull(result);
            string res = (string) result?.Value!;
            Assert.Equal("The password has been changed", res);
            
            //Verify
            _mockUserService.Verify(mock => mock.ChangePasswordUser(id,actualPassword,newPassword,passwordConfirmation), Times.Once);
        }
    }
}