using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class InstitutionController : ControllerBase
    {
        // get all the institutions
        public static async Task<IResult> getInstitutions(GsdsDb db)
        {
            return TypedResults.Ok(await db.Institutions.ToArrayAsync());
        }
    }
}
