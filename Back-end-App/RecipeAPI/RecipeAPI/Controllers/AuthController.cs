using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using RecipeAPIModel.DAL.Interfaces;

namespace RecipeAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /*private UserManager<User> _userManager; 
        public AuthController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }*/

        private readonly IDALUser _user;

        public AuthController(IDALUser user)
        {
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] LoginModel model)
        {
            var user = new User();
            user.mail = model.email;
            user.password = model.password;
            await _user.InsertUser(user);
            return Ok();
        }

            [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Revisar usuario
            //var user = await _userManager.FindByNameAsync(model.email);
            //if (user != null && await _userManager.CheckPasswordAsync(user, model.password))
            var user = await _user.GetUser(model.email);

            if (model != null && user != null && (model.password == user.password))
            {
                var authClaims = new []
                {
                     new Claim(JwtRegisteredClaimNames.Sub, model.email),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RecipeAPISigningCredentials"));

                var token = new JwtSecurityToken(
                    issuer: "RecipeAPI",
                    audience: "http://localhost:4200/",
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}
