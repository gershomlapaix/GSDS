using Gsds.Data;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class IdentifierTypeController : ControllerBase
    {
        public static async Task<IResult> getAllIdentifierTypes(GsdsDb db)
        {
            return TypedResults.Ok(await db.IdentifierTypes.ToArrayAsync());
        }

        public static async Task<IResult> getIdentifierTypeById(string id, GsdsDb db)
        {
            return await db.IdentifierTypes.Where(c => c.Id == id).ToListAsync()
               is List<IdentifierType> identifierTypes
           ? TypedResults.Ok(identifierTypes[0])
           : TypedResults.NotFound();
        }
    }
}
