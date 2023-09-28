// This class contains methods like user
using Gsds.Data;
using GsdsAuth.Models;
using GsdsV2.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gsds.Controllers.Auth{
    public class UserController: ControllerBase{

        // UserRegister method
        public static async Task<IResult> UserRegister(UserDto userDto, GsdsDb db){
            var newUser = new User()
            {
                Username = userDto.Username,
                Password = System.Text.Encoding.UTF8.GetBytes(userDto.Password),
                email = userDto.email,
                FullName = userDto.FullName,
                Phone = userDto.Phone,
                // default properties
                status = 1,
                GroupId = 2,
                ID_ROLE = "00049",
                DEPARTMENT_ID = 1,
                EMPID = "0"
            };

            db.Users.Add(newUser);
            await db.SaveChangesAsync();

            userDto = new UserDto(newUser);

            return TypedResults.Created($"/api/auth/{userDto.email}", userDto);
        }

        // get complaints by the role id
        public static async Task<IResult> getMyRoleComplaints(ClaimsPrincipal user, GsdsDb db)
        {
            var complaints = await db.Complaints.Where(c => c.RoleId == user.FindFirstValue(ClaimTypes.Role)).ToListAsync();
            return TypedResults.Ok(complaints);
        }
    }
}