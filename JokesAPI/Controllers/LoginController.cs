using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JokesAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace JokesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly AppDbContext _dbContext;
        private IConfiguration _config;
        private readonly ILogger _log;

        public LoginController(IConfiguration config, AppDbContext context, ILogger<JokesController> logger)
        {
            _config = config;
            _dbContext = context;
            _log = logger;
        }

        /// <summary>
        /// Logs the user in and returns a JWT Java Web Token
        /// </summary>
        /// <param name="username">name of an existing user</param>
        /// <param name="password"></param>
        /// <returns>JWT Web Token if the user exists</returns>
        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            IActionResult response = Unauthorized();

            try
            {
                UserAuth login = new UserAuth();
                login.UserName = username;
                login.Password = password;

                var user = AuthenticateUser(login);

                if (user != null)
                {
                    var tokenStr = GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenStr });
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception during login.");
            }

            return response;
        }

        /// <summary>
        /// Generates the JWT for a given UserAuth object
        /// </summary>
        /// <param name="user">A UserAuth object used to generate the JWT</param>
        /// <returns>A JWT Security Token</returns>
        private string GenerateJSONWebToken(UserAuth user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);


            return encodetoken;
        }

        /// <summary>
        /// Authenticates the user
        /// </summary>
        /// <param name="login">A UserAuth object containing the UserName and Password</param>
        /// <returns>UserAuth object</returns>
        private UserAuth AuthenticateUser(UserAuth login)
        {
            UserAuth user = null;

            try
            {

                List<UserInfo> newUser = _dbContext.UserInfo.Where<UserInfo>(x => x.UserName == login.UserName && x.Password == login.Password).ToList();

                if (newUser.Count != 0)
                {
                    _log.LogInformation("User {UserName} was authenticated", login.UserName);

                    user = new UserAuth { UserName = newUser[0].UserName, EmailAddress = newUser[0].Email, Password = newUser[0].Password };
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception during authentication of user={UserName} during Login", login.UserName);
            }

            return user;

        }

        /// <summary>
        /// Welcome the new login...just here for testing
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome To: " + userName;
        }

    }
}