using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier
{
    public class AccusedController : Controller
    {
       // Get all accuseds
       public static async Task<IResult> getAllAccuseds(GsdsDb db)
        {
            return TypedResults.Ok(await db.Accuseds.ToArrayAsync());

        }
    }
}
