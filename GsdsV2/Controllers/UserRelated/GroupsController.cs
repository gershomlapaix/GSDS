using Gsds.Data;
using GsdsV2.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GsdsV2.Controllers.UserRelated
{
    public class GroupsController : ControllerBase
    {
        public static async Task<IResult> GetAllGroups( GsdsDb db)
        {
            var groups = await db.UserGroups.ToListAsync();
            return TypedResults.Ok(groups);
        }

        //Get One department
        public static async Task<IResult> GetgroupById(string groupId, GsdsDb db)
        {
            return await db.UserGroups.Where(c => c.GroupID
            == int.Parse(groupId)).ToListAsync()
               is List<UserGroup> theGroup
           ? TypedResults.Ok(theGroup[0])
           : TypedResults.NotFound();

        }
    }
}
