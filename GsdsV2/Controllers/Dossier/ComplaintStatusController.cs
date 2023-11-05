using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintStatusController : ControllerBase
    {
        // get all statuses
        public static async Task<IResult> GetAll(GsdsDb db)
        {
            var statuses = await db.ComplaintStatuses.ToListAsync();
            return TypedResults.Ok(statuses);
        }

        //Get One complaints
        public static async Task<IResult> GetByCode(string statusCode, GsdsDb db)
        {
            return TypedResults.Ok(await db.ComplaintStatuses
                .Where(c => c.Id == statusCode)
                .ToListAsync());
        }
    }
}
