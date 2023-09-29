using Gsds.Data;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class MaritalStatusController : ControllerBase
    {
        public static async Task<IResult> getallMaritalStatus(GsdsDb db)
        {
            return TypedResults.Ok(await db.MaritalStatuses.ToArrayAsync());
        }

        public static async Task<IResult> getMaritalStatusById(string id, GsdsDb db)
        {
            return await db.MaritalStatuses.Where(c => c.Id == id).ToListAsync()
               is List<MaritalStatus> maritalStatus
           ? TypedResults.Ok(maritalStatus[0])
           : TypedResults.NotFound();
        }
    }
}
