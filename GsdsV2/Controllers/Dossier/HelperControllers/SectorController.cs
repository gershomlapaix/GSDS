using Gsds.Data;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class SectorController : ControllerBase
    {
        // Get the corresponding districts'
        public static async Task<IResult> getCellBySector(string sectorId, GsdsDb db)
        {
            var cells = await db.Sectors.Where(_ => _.Id == sectorId).Include(_ => _.Cells).ToListAsync();

            return TypedResults.Ok(cells);
        }

        // Get one sector
        public static async Task<IResult> getSectorById(string sectorId, GsdsDb db)
        {
            var sectors = await db.Sectors.Where(_ => _.Id == sectorId).ToListAsync();

            return TypedResults.Ok(sectors[0]);
        }

        // Get the complaints by the province
        public static async Task<IResult> getComplaintsBySector(string sectorId, GsdsDb db)
        {
            var complaints = await db.Sectors.Where(_ => _.Id == sectorId).Include(c => c.Complaints).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the complainers
        public static async Task<IResult> getComplainersBySector(string sectorId, GsdsDb db)
        {
            var complaints = await db.Sectors.Where(_ => _.Id == sectorId).Include(c => c.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the accuseds
        public static async Task<IResult> getAccusedBySector(string sectorId, GsdsDb db)
        {
            var complaints = await db.Sectors.Where(_ => _.Id == sectorId).Include(_ => _.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }
    }
}
