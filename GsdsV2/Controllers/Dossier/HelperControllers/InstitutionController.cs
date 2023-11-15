using Gsds.Data;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public static async Task<IResult> GetAssignedComplaints(string institutionId, ClaimsPrincipal user, GsdsDb db)
        {
            var complaints = await db.Institutions.Where(_ => _.Id == float.Parse(institutionId) && _.RoleId == user.FindFirstValue(ClaimTypes.Role))
                .Select(i => new 
                { 
                    i.Id,
                    i.InstitutionName,
                    i.Email,
                    i.Complaints
                }).ToListAsync();

            return TypedResults.Ok(complaints);
        }
    }
}
