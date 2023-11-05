// This class contains methods like user
using Gsds.Data;
using GsdsAuth.Models;
using GsdsV2.DTO;
using GsdsV2.Models.Users;
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
                ID_ROLE = "00052",
                DepartmentId = "1",
                EMPID = "0"
            };

            db.Users.Add(newUser);
            await db.SaveChangesAsync();

            userDto = new UserDto(newUser);

            return TypedResults.Created($"/api/auth/{userDto.email}", userDto);
        }

        public static async Task<IResult> UserRegisterByAdmin(UserRegisterDtoAdmin userDto, GsdsDb db)
        {
            var newUser = new User()
            {
                Username = userDto.Username,
                Password = System.Text.Encoding.UTF8.GetBytes(userDto.Password),
                email = userDto.email,
                FullName = userDto.FullName,
                Phone = userDto.Phone,
                // default properties
                status = 1,
                GroupId = userDto.GroupId,
                ID_ROLE = userDto.RoleId,
                DepartmentId = userDto.DepartmentId,
                EMPID = "113"
            };

            db.Users.Add(newUser);
            await db.SaveChangesAsync();

            UserRegisterDtoAdmin user = new UserRegisterDtoAdmin(newUser);

            return TypedResults.Created($"/api/auth/{userDto.email}", userDto);
        }

        // get logged in user
        public static async Task<IResult> getLoggedInUser(ClaimsPrincipal user, GsdsDb db)
        {
            var newUser = new User();
            newUser.Username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            newUser.email = user.FindFirstValue(ClaimTypes.Email);
            newUser.FullName = user.FindFirstValue(ClaimTypes.GivenName);
            newUser.ID_ROLE = user.FindFirstValue(ClaimTypes.Role);

            return TypedResults.Ok(newUser);
        }

        // get all complaints
        public static async Task<IResult> GetAllUsers(GsdsDb db)
        {
            var users = await db.Users
                .Select(u =>
                new {
                    u.Username,
                    u.FullName,
                    u.ManagerRoles.RoleName,
                    u.status,
                    u.email,
                    u.Phone,
                   u.TheGroup.groupName,
                   u.Department.DepartmentName
                })
                .ToListAsync();
            return TypedResults.Ok(users);
        }

        // get complaints by the role id
        public static async Task<IResult> getMyRoleComplaints(ClaimsPrincipal user, GsdsDb db)
        {
            var complaints = await db.Complaints.Where(c => c.RoleId == user.FindFirstValue(ClaimTypes.Role)).ToListAsync();
            return TypedResults.Ok(complaints);
        }
    }
}