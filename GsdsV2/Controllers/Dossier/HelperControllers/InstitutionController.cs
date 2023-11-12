using Gsds.Data;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class InstitutionController : ControllerBase
    {
        // get all the institutions
        public static async Task<IResult> GetInstitutions(GsdsDb db)
        {
            return TypedResults.Ok(await db.Institutions.ToArrayAsync());
        }

        public static async Task<IResult> GetInstitutionById(Double institutionId, GsdsDb db)
        {
            var institutions = await db.Institutions
                .Where(_ => _.Id == institutionId)
                .ToListAsync();

            if(institutions.Count() > 0)
            {
                return TypedResults.Ok(institutions[0]);
            }
            else
            {
                return TypedResults.NotFound(" Institution Not found");
            }
        }

        // Get all the complaints assigned to an institution
        public static async Task<IResult> GetAssignedComplaints(float institutionId, GsdsDb db)
        {
            var complaints = await db.Institutions.Where(_ => _.Id == institutionId)
                .Select(i => new 
                { 
                    i.Complaints

                }).ToListAsync();

            return TypedResults.Ok(complaints);
        }
    }
}
