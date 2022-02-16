using DuckCity.Application.UserService;
using DuckCity.Application.Utils;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.UserRepository;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Application
{
    public class UserServiceUt
    {
        // Class to test
        private readonly UserService _userService;
        
        // Mock
        private readonly Mock<IUserRepository> _mockUserRep = new();

        // Constructor
        public UserServiceUt()
        {
            _userService = new UserService(_mockUserRep.Object);
        }

        /**
         * Tests
         */
        
        [Theory]
        [InlineData("something not ObjectId")]
        [InlineData(ConstantTest.UserId)]
        public void DeleteUserTest(string userId)
        {
            _mockUserRep.Setup(mock => mock.CountUserById(userId)).Returns(1);
            _mockUserRep.Setup(mock => mock.DeleteUserById(userId));
            try
            {
                _userService.DeleteAccountUser(userId);
                _mockUserRep.Verify(mock => mock.CountUserById(userId), Times.Once);
                _mockUserRep.Verify(mock => mock.DeleteUserById(userId), Times.Once);
            }
            catch (IdNotValidException e)
            {
                Assert.True(!ObjectId.TryParse(userId, out _));
                Assert.NotNull(e);
                _mockUserRep.Verify(mock => mock.DeleteUserById(userId), Times.Never);
            }
        }
        
        [Theory]
        [InlineData("something not ObjectId",ConstantTest.String,ConstantTest.String,ConstantTest.String)]
        [InlineData(ConstantTest.UserId,"password",ConstantTest.String,ConstantTest.String)]
        [InlineData(ConstantTest.UserId,"password","a String","anotherString")]
        [InlineData(ConstantTest.UserId,"BadPassword",ConstantTest.String,ConstantTest.String)]
        public void ChangePasswordUserTest(string userId, string actualPassword, string newPassword,
            string newPasswordConfirmation)
        {
            //Given
            string password = "password";
            string hashedPassword = UserUtils.HashPassword(password);
            User user = new User("aName", "aMail", UserUtils.HashPassword(newPassword)) {Id = userId};
            _mockUserRep.Setup(mock => mock.CountUserById(userId)).Returns(1);
            _mockUserRep.Setup(mock => mock.FindById(userId)).Returns(user);
            user.Password = hashedPassword;
            _mockUserRep.Setup(mock => mock.Replace(user));

            try
            {
                _userService.ChangePasswordUser(userId, actualPassword, newPassword, newPasswordConfirmation);
                _mockUserRep.Verify(mock => mock.Replace(user), Times.Once);
            }
            catch (IdNotValidException e)
            {
                Assert.True(!ObjectId.TryParse(userId, out _));
                Assert.NotNull(e);
                _mockUserRep.Verify(mock => mock.Replace(user), Times.Never);
            }
            catch (PasswordConfirmationException e)
            {
                Assert.NotNull(e);
                Assert.NotEqual(newPassword, newPasswordConfirmation);
                _mockUserRep.Verify(mock => mock.Replace(user), Times.Never);
            }
            catch (BadUserOrPasswordException e)
            {
                Assert.NotNull(e);
                Assert.NotEqual(password, actualPassword);
                _mockUserRep.Verify(mock => mock.Replace(user), Times.Never);
            }
        }
    }
}