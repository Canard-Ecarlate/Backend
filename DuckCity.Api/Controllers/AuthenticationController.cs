using AutoMapper;
using DuckCity.Api.DTO.Authentication;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace DuckCity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        public ActionResult<UserWithTokenDto> Login(IdentifierDto identifierDto)
        {
            User user = _authenticationService.Login(identifierDto.Name, identifierDto.Password);
            UserWithTokenDto userWithTokenDto = _mapper.Map<UserWithTokenDto>(user);
            userWithTokenDto.Token = _authenticationService.GenerateJsonWebToken(user);
            return new OkObjectResult(userWithTokenDto);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<UserWithTokenDto> SignUp(RegisterDto registerDto)
        {
            _authenticationService.SignUp(registerDto.Name, registerDto.Email, registerDto.Password, registerDto.PasswordConfirmation);
            return Login(new IdentifierDto {Name = registerDto.Name, Password = registerDto.Password});
        }
    }
}
