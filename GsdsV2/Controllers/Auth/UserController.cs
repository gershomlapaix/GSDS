// This class contains methods like user
using Gsds.Data;
using GsdsAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gsds.Controllers.Auth{
    public class UserController: ControllerBase{

        // UserRegister method
        public static async Task<IResult> UserRegister(User user, GsdsDb db){
            db.Users.Add(user);
                    await db.SaveChangesAsync();

                    return TypedResults.Created($"/api/users/{user.Email}", user);
        }
        // .Accepts<UserLogin>("application/json")
        // .Produces<string>();
    }
}