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
    }
}
