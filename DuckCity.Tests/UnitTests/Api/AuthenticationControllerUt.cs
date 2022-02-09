﻿using System;
using AutoMapper;
using DuckCity.Api.Controllers;
using DuckCity.Api.DTO.Authentication;
using DuckCity.Application.AuthenticationService;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace DuckCity.Tests.UnitTests.Api
{
    public class AuthenticationControllerUt
    {
        // Class to test
        private readonly AuthenticationController _authenticationController;

        // Mock
        private readonly Mock<IAuthenticationService> _mockAuthenticationService = new();
        private readonly Mock<IMapper> _mockMapper = new();

        // Constructor
        public AuthenticationControllerUt()
        {
            _authenticationController =
                new AuthenticationController(_mockAuthenticationService.Object, _mockMapper.Object)
                {
                    ControllerContext = {HttpContext = new Mock<HttpContext>().Object}
                };
        }

        /**
         * Tests
         */
        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.String, ConstantTest.String)]
        public void LoginTest(string name, string password, string token)
        {
            // Given
            IdentifierDto identifierDto = new() {Name = name, Password = password};
            User user = new(name, "", password);
            _mockAuthenticationService.Setup(mock => mock.Login(name, password)).Returns(user);
            _mockMapper.Setup(mock => mock.Map<UserWithTokenDto>(user)).Returns(new UserWithTokenDto {Name = name});
            _mockAuthenticationService.Setup(mock => mock.GenerateJsonWebToken(user)).Returns(token);

            // When
            ActionResult<UserWithTokenDto> actionResult = _authenticationController.Login(identifierDto);
            OkObjectResult? result = actionResult.Result as OkObjectResult;

            // Then
            Assert.NotNull(result);
            UserWithTokenDto res = (UserWithTokenDto) result?.Value!;
            Assert.Equal(name, res.Name);
            Assert.Equal(token, res.Token);

            // Verify
            _mockAuthenticationService.Verify(mock => mock.Login(name, password), Times.Once);
            _mockAuthenticationService.Verify(mock => mock.GenerateJsonWebToken(user), Times.Once);
            _mockMapper.Verify(mock => mock.Map<UserWithTokenDto>(user), Times.Once);
        }

        [Fact]
        public void LoginUnauthorizedTest()
        {
            // Mock
            _mockAuthenticationService.Setup(mock => mock.Login(It.IsAny<string?>(), It.IsAny<string?>()))
                .Throws(new BadUserOrPasswordException());

            try
            { 
                // When
                _authenticationController.Login(new IdentifierDto());
                throw new TestMustBeFailedException();
            }
            catch (BadUserOrPasswordException e)
            {
                // Then
                Assert.NotNull(e);
            }

            // Verify
            _mockAuthenticationService.Verify(mock => mock.Login(It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
        }

        [Theory]
        [InlineData(ConstantTest.String, ConstantTest.Email, ConstantTest.String, ConstantTest.UserId,
            ConstantTest.String)]
        public void SignUpTest(string name, string email, string password, string id, string token)
        {
            // Given
            //      SignUp
            RegisterDto registerDto = new()
                {Name = name, Email = email, Password = password, PasswordConfirmation = password};
            //      Login
            User user = new(name, "", password);
            
            // Mock
            _mockAuthenticationService.Setup(mock => mock.SignUp(name, email, password, password));
            _mockAuthenticationService.Setup(mock => mock.Login(name, password)).Returns(user);
            _mockAuthenticationService.Setup(mock => mock.GenerateJsonWebToken(user)).Returns(token);
            _mockMapper.Setup(mock => mock.Map<UserWithTokenDto>(user))
                .Returns(new UserWithTokenDto {Name = name, Email = email, Id = id});

            // When
            ActionResult<UserWithTokenDto> actionResult = _authenticationController.SignUp(registerDto);
            OkObjectResult? result = actionResult.Result as OkObjectResult;

            // Then
            Assert.NotNull(result);
            UserWithTokenDto res = (UserWithTokenDto) result?.Value!;
            Assert.Equal(name, res.Name);
            Assert.Equal(email, res.Email);
            Assert.True(ObjectId.TryParse(res.Id, out _));
            Assert.NotEmpty(res.Token!);

            // Verify
            _mockAuthenticationService.Verify(mock => mock.SignUp(name, email, password, password), Times.Once);
            _mockAuthenticationService.Verify(mock => mock.Login(name, password), Times.Once);
            _mockAuthenticationService.Verify(mock => mock.GenerateJsonWebToken(user), Times.Once);
            _mockMapper.Verify(mock => mock.Map<UserWithTokenDto>(user), Times.Once);
        }
    }
}