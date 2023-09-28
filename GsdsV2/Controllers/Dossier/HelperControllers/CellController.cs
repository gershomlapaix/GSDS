using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class CellController : ControllerBase
    {
        // Get the complaints by the province
        public static async Task<IResult> getComplaintsByCell(string cellId, GsdsDb db)
        {
            var complaints = await db.Cells.Where(_ => _.Id == cellId).Include(c => c.Complaints).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the complainers
        public static async Task<IResult> getComplainersByCell(string cellId, GsdsDb db)
        {
            var complaints = await db.Cells.Where(_ => _.Id == cellId).Include(c => c.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the accuseds
        public static async Task<IResult> getAccusedByCell(string cellId, GsdsDb db)
        {
            var complaints = await db.Cells.Where(_ => _.Id == cellId).Include(_ => _.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }
    }
}
