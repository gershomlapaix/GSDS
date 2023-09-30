using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class ComplaintCategoryController : ControllerBase
    {
        // get all categories
        public static async Task<IResult> getComplaintCategories(GsdsDb db)
        {
            return TypedResults.Ok(await db.complaintCategories.ToArrayAsync());
        }
    }
}
