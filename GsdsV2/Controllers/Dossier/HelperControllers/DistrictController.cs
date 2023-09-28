using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class DistrictController : ControllerBase
    {

        // Get the corresponding districts'
        public static async Task<IResult> getSectorByDistrict(string districtId, GsdsDb db)
        {
            var sectors = await db.Districts.Where(_ => _.Id == districtId).Include(_ => _.Sectors).ToListAsync();

            return TypedResults.Ok(sectors);
        }

        // Get the complaints by the province
        public static async Task<IResult> getComplaintsByDistrict(string districtId, GsdsDb db)
        {
            var complaints = await db.Districts.Where(_ => _.Id == districtId).Include(c => c.Complaints).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the complainers
        public static async Task<IResult> getComplainersByDistrict(string districtId, GsdsDb db)
        {
            var complaints = await db.Districts.Where(_ => _.Id == districtId).Include(c => c.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the accuseds
        public static async Task<IResult> getAccusedByDistrict(string districtId, GsdsDb db)
        {
            var complaints = await db.Districts.Where(_ => _.Id == districtId).Include(_ => _.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }

    }
}
