using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class ProvinceController : ControllerBase
    {
        // Get all the provinces
        public static async Task<IResult> getProvinces( GsdsDb db)
        {
            var provinces = await db.Provinces.ToListAsync();

            return TypedResults.Ok(provinces);
        }

        // Get the corresponding districts'
        public static async Task<IResult> getDistrictsByProvince(string provinceId, GsdsDb db)
        {
            var districits = await db.Provinces.Where(p => p.Id == provinceId).Include(d => d.Districts).ToListAsync();

            return TypedResults.Ok(districits);
        }

        // Get the complaints by the province
        public static async Task<IResult> getComplaintsByProvince(string provinceId, GsdsDb db)
        {
            var complaints = await db.Provinces.Where(p => p.Id == provinceId).Include(c => c.Complaints).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the complainers
        public static async Task<IResult> getComplainersByProvince(string provinceId, GsdsDb db)
        {
            var complaints = await db.Provinces.Where(p => p.Id == provinceId).Include(c => c.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }

        // Get the accuseds
        public static async Task<IResult> getAccusedByProvince(string provinceId, GsdsDb db)
        {
            var complaints = await db.Provinces.Where(p => p.Id == provinceId).Include(c => c.Complainers).ToListAsync();

            return TypedResults.Ok(complaints);
        }
    }
}
