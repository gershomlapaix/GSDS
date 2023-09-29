using Gsds.Data;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class PersonTypeController : ControllerBase
    {
        public static async Task<IResult> getPersonTypes(GsdsDb db)
        {
            return TypedResults.Ok(await db.PersonTypes.ToArrayAsync());
        }

        public static async Task<IResult> getPersonTypeController(string id, GsdsDb db)
        {
            return await db.PersonTypes.Where(c => c.Id == id).ToListAsync()
               is List<PersonType> personTypes
           ? TypedResults.Ok(personTypes[0])
           : TypedResults.NotFound();
        }
    }
}
