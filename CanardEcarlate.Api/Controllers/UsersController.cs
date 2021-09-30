using CanardEcarlate.Application;
using CanardEcarlate.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CanardEcarlate.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanardEcarlate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
 
        public UsersController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public ActionResult<List<User>> Get() =>
            _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            _userService.Create(user);

            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User userIn)
        {
            userIn.Id = id;
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Remove(_userService.Get(id));

            return NoContent();
        }


        [HttpPost]
        [Route("[action]")]
        public ActionResult<UserWithToken> Login([FromBody] User User)
        {
            User userAuthentifiate = null;
            try
            {
                userAuthentifiate = _userService.Login(User.Name, User.Password);
            }
            catch (UnauthorizedAccessException e) {
                return Unauthorized();
            }

            var Claims = new List<Claim>
            {
                new Claim("type", "Joueur"),
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

            var Token = new JwtSecurityToken(
                "https://canardecarlate.fr",
                "https://canardecarlate.fr",
                Claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            );
            UserWithToken userToken = new UserWithToken { token = new JwtSecurityTokenHandler().WriteToken(Token), user = userAuthentifiate };
            return new OkObjectResult(userToken);
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<User> SignUp(String name,String mail, String password, String passwordConfirmation) {
            try {
                User user = _userService.SignUp(name, mail, password, passwordConfirmation);
                return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
            }
            catch (Exception e) {
                Console.Write(e.Message);
                throw;
            }
        }
    }
}
