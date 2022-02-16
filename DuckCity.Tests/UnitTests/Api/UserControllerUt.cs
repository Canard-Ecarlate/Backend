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
        // Constructor
        public UserControllerUt()
        {
            _userController =
                new UserController(_mockUserService.Object)
                {
                    ControllerContext = {HttpContext = new Mock<HttpContext>().Object}
                };
        }

        /**
     * Tests
     */
        [Theory]
        [InlineData(ConstantTest.UserId, ConstantTest.String, ConstantTest.Email)]
        public void DeleteUserTest(string id, string name, string mail)
        {
            //Given
            UserDto userDto = new() { UserId = id,UserName = name,UserMail = mail};
            _mockUserService.Setup(mock => mock.DeleteAccountUser(id));
            
            //When
            ActionResult<string> actionResult = _userController.DeleteUser(userDto);
            OkObjectResult? result = actionResult.Result as OkObjectResult;
            
            //Then
            Assert.NotNull(result);
            string res = (string) result?.Value!;
            Assert.Equal("The account is deleted", res);
            
            //Verify
            _mockUserService.Verify(mock => mock.DeleteAccountUser(id), Times.Once);
        }
        
        [Theory]
        [InlineData(ConstantTest.UserId, ConstantTest.String, ConstantTest.String,ConstantTest.String)]
        public void ChangePasswordTest(string id, string actualPassword, string newPassword, string passwordConfirmation)
        {
            //Given
            UserChangePassword userChangePassword = new() {UserId = id,ActualPassword = actualPassword,NewPassword = newPassword,PasswordConfirmation = passwordConfirmation};
            _mockUserService.Setup(mock => mock.ChangePasswordUser(id,actualPassword,newPassword,passwordConfirmation));
            
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