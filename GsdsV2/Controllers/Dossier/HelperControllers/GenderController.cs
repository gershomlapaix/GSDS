using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class GenderController : ControllerBase
    {
        // get genders
        public static async Task<IResult> getGenders(GsdsDb db)
        {
            return TypedResults.Ok(await db.Gender.ToArrayAsync());
        }

        // get one gender
        public static async Task<IResult> getGenderById(string genderId, GsdsDb db)
        {
            var gender = await db.Gender.Where(_ => _.Id == genderId).ToListAsync();
            return TypedResults.Ok(gender[0]);
        }
    }
}
