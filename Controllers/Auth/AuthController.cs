// Controller for authentication controller

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gsds.Auth;
using Gsds.Models;
using Gsds.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Gsds.Controller.Auth{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController: ControllerBase{
        
        // helpful object properties
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJwtBearerServices
        private readonly IConfiguration _configuration;

        // dependency injection with constructor
        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration){
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        // Login method
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel){
            var user = await userManager.FindByNameAsync(loginModel.username);
            
            // if user is found
            if(user != null && await userManager.CheckPasswordAsync(user,loginModel.Password)){
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach(var userRole in userRoles){
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration
                )
            }
        }
    }
}