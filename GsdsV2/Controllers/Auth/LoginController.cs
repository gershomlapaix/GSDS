// This class contains the asynchronous login method which returns a token

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gsds.Data;
using GsdsAuth.Models;
using GsdsAuth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;

namespace Gsds.Controllers.Auth
{
    public class LoginController : ControllerBase{


        // Login method
        public static async Task<IResult> Login(WebApplicationBuilder builder, UserLogin user, IUserService service){
            if(!string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password)){
                var loggerInUser = service.GetUser(user);
                // var loggerInUser = await db.Users.FindAsync(user.Username);


                if(loggerInUser is null) {
                    return TypedResults.NotFound("User not found");
                }

                // else

                var claims = new[]{
                    new Claim(ClaimTypes.NameIdentifier, loggerInUser.Username),
                    //new Claim(ClaimTypes.Email, loggerInUser.Email),
                    //new Claim(ClaimTypes.GivenName, loggerInUser.FirstName),
                    //new Claim(ClaimTypes.Surname, loggerInUser.LastName),
                    //new Claim(ClaimTypes.Role, loggerInUser.Role)
                };

                // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
                // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                issuer: builder.Configuration["Jwt:Issuer"],
                audience: builder.Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                notBefore:DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return TypedResults.Ok(tokenString);
            }
            else{
                return TypedResults.BadRequest("Enter the data");
            }
        }
    }
}