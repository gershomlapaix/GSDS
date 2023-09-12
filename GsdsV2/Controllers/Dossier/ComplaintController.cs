using Gsds.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintController : Controller
    {
       // Get all complaints
       public static async Task<IResult> getAllComplaints(GsdsDb db)
        {
            return TypedResults.Ok(await db.Complaints.ToArrayAsync());
        }
    }
}
