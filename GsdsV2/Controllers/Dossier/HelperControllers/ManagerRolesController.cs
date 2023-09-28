using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class ManagerRolesController : ControllerBase
    {
        // Get all the roles
        public static async Task<IResult> getAllRoles(GsdsDb db)
        {
            return TypedResults.Ok(await db.ManagerRoles.ToArrayAsync());
        }

        // get the complaints corresponding to a certain role
        public static async Task<IResult> getRoleComplaints(string roleId, GsdsDb db)
        {
            var complaints = await db.ManagerRoles.Where(r => r.Id == roleId).Include(_ => _.Complaints).ToListAsync();
            //.Include(_ => _.Complaints).ToListAsync();
            return TypedResults.Ok(complaints);
        }
    }
}
