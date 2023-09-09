// This class contains the asynchronous login method which returns a token

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gsds.Data;
using GsdsAuth.Models;
using GsdsAuth.Services;
using GsdsV2.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;

namespace Gsds.Controllers.Auth
{
    public class LoginController : ControllerBase{


        // Login method
        public static async Task<IResult> Login(WebApplicationBuilder builder, UserLogin userLogin, GsdsDb db){
            if (!string.IsNullOrEmpty(userLogin.Username) && !string.IsNullOrEmpty(userLogin.Password)) {

                var foundUser = await db.Users.Where(u => u.Username== userLogin.Username).ToListAsync();
                if (!foundUser.Any())
                {
                    return TypedResults.NotFound("User not found");
                }

                else
                {
                    if (userLogin.Password == System.Text.Encoding.UTF8.GetString(foundUser[0].Password))
                    {
                        var claims = new[]{
                            new Claim(ClaimTypes.NameIdentifier, foundUser[0].Username),
                            new Claim(ClaimTypes.Email, foundUser[0].email),
                            new Claim(ClaimTypes.GivenName, foundUser[0].FullName),
                            new Claim(ClaimTypes.Role, foundUser[0].ID_ROLE)
                        };

                        // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
                        // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                        issuer: builder.Configuration["Jwt:Issuer"],
                        audience: builder.Configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(15),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                        SecurityAlgorithms.HmacSha256)
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                        return TypedResults.Ok(tokenString);
                    }
                    return TypedResults.Ok("Incorrect username or password");

                    
                }
            }
            else{
                return TypedResults.BadRequest("Enter the data");
            }
        }
    }
}