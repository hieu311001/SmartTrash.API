using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using SmartTrash.API.Entity;
using SmartTrash.API.Function;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartTrash.API.Controller
{
    public class UserController : BaseController<User>
    {
        public UserFunction userFunc = new UserFunction();
        public UserController(IConfiguration configuration) : base(configuration)
        {

        }

        [HttpPost("signin")]
        public IActionResult Signin([FromBody] User model)
        {
            var result = userFunc.CheckSignIn(model.UserName, model.Password);
            if (result == null)
            {
                return BadRequest("Invalid username or password");
            }

            var user = userFunc.GetByName(model.UserName);
            var token = userFunc.GenerateToken(user);

            return Ok(token);
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] User model)
        {
            var result = userFunc.SignUp(model);
            if (result == 0)
            {
                return BadRequest("Thêm user thất bại!");
            }
            return Ok("User created successfully");
        }

        [HttpPost("signout")]
        public IActionResult SignOut(string userName)
        {
            userFunc.SignOut(userName);
            // Return a success message
            return Ok("Signed out successfully.");
        }
    }
}
