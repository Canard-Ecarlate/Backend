using CanardEcarlate.Application;
using CanardEcarlate.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CanardEcarlate.Api.Models;
using AutoMapper;

namespace CanardEcarlate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(AuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        public ActionResult<UserWithToken> Login(Identifier identifier)
        {
            User user;
            try
            {
                user = _authenticationService.Login(identifier.Name, identifier.Password);
            }
            catch (UnauthorizedAccessException e) {
                return Unauthorized(e);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("type", "player")
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

            JwtSecurityToken token = new JwtSecurityToken(
                "https://canardecarlate.fr",
                "https://canardecarlate.fr",
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            
            UserWithToken userWithToken = _mapper.Map<UserWithToken>(user);
            userWithToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
            
            return new OkObjectResult(userWithToken);
        }

        [HttpPost]
        [Route("")]
        public ActionResult SignUp(Register register) {
            try {
                _authenticationService.SignUp(register.Name, register.Email, register.Password, register.PasswordConfirmation);
                return new OkResult();
            }
            catch (Exception e) {
                Console.Write(e.Message);
                throw;
            }
        }
    }
}
