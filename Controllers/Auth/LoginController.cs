// Controller for authentication controller
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gsds.Data;
using Gsds.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gsds.Controller.Auth{

// For handling login
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController: ControllerBase{
       private IConfiguration _config;

       public LoginController(IConfiguration config){
        _config = config;
       }

       [AllowAnonymous] // to declare that this class does not need authorization
       [HttpPost]
       public IActionResult Login([FromBody] UserLogin userLogin){
        var user = Authenticate(userLogin);

        if(user != null){
            var token = Generate(user);
            return Ok(token);
        }

        return NotFound("User not found");
       }

    //    Authenticate method to check if the login credentials match any stored users' credentials
       private UserModel Authenticate(UserLogin userLogin){
        var currentUser = UserData.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);

        if(currentUser != null){
            return currentUser;
        }

        return null;
       }

       private string Generate(UserModel user){
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // store the trusted attributes about the user
        var claims = new[]{
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,
        expires: DateTime.Now.AddMinutes(15),
        signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
       }
    }
}