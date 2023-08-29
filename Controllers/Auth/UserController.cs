using System.Security.Claims;
using Gsds.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gsds.Controller.Auth{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController:ControllerBase{

        // public endpoint
        [HttpGet("Public")]
        public IActionResult Public(){
            return Ok("You're accessing the public");
        }

        // Admins endpoint
        [HttpGet("Admins")]
        [Authorize(Roles ="Administrator")]
        public IActionResult AdminsEndPoint(){
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.FirstName}");
        }

        // Admins endpoint
        [HttpGet("forall")]
        [Authorize(Roles ="Administrator, User")]
        public IActionResult AdminsandUsersEndpoint(){
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.FirstName}, your role is {currentUser.Role}");
        }

        // get the current user
        private UserModel GetCurrentUser(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity != null){
                var userClaims = identity.Claims;

                return new UserModel{
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    FirstName = userClaims.FirstOrDefault(o=>o.Type == ClaimTypes.GivenName)?.Value,
                    LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o=> o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}