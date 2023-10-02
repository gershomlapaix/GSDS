using Gsds.Data;
using GsdsV2.Models.HelperModels;
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

        //get one cell
        public static async Task<IResult> getCellById(string cellId, GsdsDb db)
        {
            var cells = await db.Cells.Where(_ => _.Id == cellId).ToListAsync();

            if (cells.Count() > 0)
            {
                return TypedResults.Ok(cells[0]);
            }
            else
            {
                return TypedResults.NotFound("Cell not found");
            }
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
