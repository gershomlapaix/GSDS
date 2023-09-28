using Gsds.Data;
using Gsds.Models.Dossier;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class CountryController : ControllerBase
    {
        public static async Task<IResult> getAllCountries(GsdsDb db)
        {
            return TypedResults.Ok(await db.Countrys.ToArrayAsync());
        }

        public static async Task<IResult> getSingleCountry(string countryId, GsdsDb db)
        {
            return await db.Countrys.Where(c => c.Id == countryId).ToListAsync()
               is List<Country> country
           ? TypedResults.Ok(country[0])
           : TypedResults.NotFound();
        }

        public static async Task<IResult> getCountryByName(string countryName, GsdsDb db)
        {
            return await db.Countrys.Where(c => c.CountryName == countryName).ToListAsync()
               is List<Country> country
           ? TypedResults.Ok(country[0])
           : TypedResults.NotFound();
        }
    }
}
