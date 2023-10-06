﻿using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class ManagerRolesController : ControllerBase
    {
        // Get all the roles
        public static async Task<IResult> GetAllRoles(GsdsDb db)
        {
            return TypedResults.Ok(await db.ManagerRoles.ToArrayAsync());
        }

        // get role by id
        public static async Task<IResult> GetRoleById(string roleId, GsdsDb db)
        {
            var roles = await db.ManagerRoles
                .Where(_ => _.Id == roleId)
                .ToListAsync();

            if (roles.Count() > 0)
            {
                return TypedResults.Ok(roles[0]);
            }
            else
            {
                return TypedResults.NotFound("Role Not found");
            }
        }

        // get the complaints corresponding to a certain role
        public static async Task<IResult> GetComplaintsByRole(string roleId, GsdsDb db)
        {
            var complaints = await db.ManagerRoles.Where(r => r.Id == roleId).Include(_ => _.Complaints).ToListAsync();
            //.Include(_ => _.Complaints).ToListAsync();
            return TypedResults.Ok(complaints);
        }
    }
}
