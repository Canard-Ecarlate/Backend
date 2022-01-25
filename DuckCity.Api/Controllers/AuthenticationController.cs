using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DuckCity.Api.DTO.Authentication;
using DuckCity.Application;
using DuckCity.Application.Services;
using DuckCity.Domain;
using DuckCity.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DuckCity.Api.Controllers
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
        public ActionResult<UserWithTokenDto> Login(IdentifierDto identifierDto)
        {
            User user;
            try
            {
                user = _authenticationService.Login(identifierDto.Name, identifierDto.Password);
            }
            catch (UnauthorizedAccessException e) {
                return Unauthorized(e);
            }

            List<Claim> claims = new()
            {
                new Claim("type", "player")
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

            JwtSecurityToken token = new(
                "https://canardecarlate.fr",
                "https://canardecarlate.fr",
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            
            UserWithTokenDto userWithTokenDto = _mapper.Map<UserWithTokenDto>(user);
            userWithTokenDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
            
            return new OkObjectResult(userWithTokenDto);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<UserWithTokenDto> SignUp(RegisterDto registerDto)
        {
            _authenticationService.SignUp(registerDto.Name, registerDto.Email, registerDto.Password, registerDto.PasswordConfirmation);
            return Login(new IdentifierDto { Name = registerDto.Name, Password = registerDto.Password });
        }
    }
}
