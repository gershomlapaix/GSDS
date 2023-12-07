using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class AttachmentDescriptionController : ControllerBase
    {
        // get files
        public static async Task<IResult> GetAttachmentDescription(GsdsDb db)
        {
            var descriptions = await db.AttachmentDescriptions
               .Select(a =>
               new {
                   a.Id,
                   a.Description
               }).ToListAsync();

            return TypedResults.Ok(descriptions);
        }
    }
}
