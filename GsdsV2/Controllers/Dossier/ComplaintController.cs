using Gsds.Data;
using Gsds.Models.Dossier;
using GsdsAuth.Models;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.Dossier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintController : ControllerBase
    {
        // Make a new complaint
        public static async Task<IResult> createComplaint(ComplaintDto complaint, GsdsDb db)
        {
            var newComplaint = new Complaint()
            {
                ComplaintCode = complaint.ComplaintCode,
                TransferDate = DateTime.Now,
                ComplainerId = complaint.ComplainerId,
                //AccusedIdNumber = complaint.AccusedIdNumber,
                Subject = complaint.Subject,
                //Attachments = complaint.Attachments,
                AccusedComment = complaint.AccusedComment,
                previousInstitutions = complaint.previousInstitutions,
                ComplaintOwner = complaint.ComplaintOwner,
                ProvinceId = complaint.ProvinceId,
                DistrictId = complaint.DistrictId,
                SectorId = complaint.SectorId,
                CellId = complaint.CellId,
                StartOffice = complaint.StartOffice,
                ComplaintCategoryId = complaint.ComplaintCategoryId,
                PriorityId = "00003"
            };

            db.Complaints.Add(newComplaint);

            await db.SaveChangesAsync();

            return TypedResults.Created($"/complainer/{complaint.ComplaintCode}", complaint);
        }


        //Get all complaints
        public static async Task<IResult> getAllComplaints(GsdsDb db)
        {
            return TypedResults.Ok(await db.Complaints.ToArrayAsync());
        }

        //Get One complaints
        public static async Task<IResult> getOneComplaint(string complaintCode, GsdsDb db)
        {
            return TypedResults.Ok(await db.Complaints
                .Where(c => c.ComplaintCode == complaintCode)
                .Include(c => c.Province)
                .ToListAsync());

        }
    }
}
