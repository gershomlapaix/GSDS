using Gsds.Data;
using GsdsV2.DTO;
using GsdsV2.DTO.Dossier;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintCloseController
    {
        // create a complaint close
        public static async Task<IResult> CreateComplaintClose(ComplaintCloseDto complaintCloseDto, GsdsDb db)
        {
            var allData = await db.ComplaintClose.ToListAsync();

            var complaintClose = new ComplaintClose();

            complaintClose.ComplaintCloseId = allData.Count() != 0 ? allData.Last().ComplaintCloseId + 1 : 1;
            complaintClose.ComplaintCode = complaintCloseDto.ComplaintCode;
            complaintCloseDto.ClosingReason = complaintCloseDto.ClosingReason;

            db.ComplaintClose.Add(complaintClose);
            var res = await db.SaveChangesAsync();

            if(res > 0)
            {
                var complaint = await db.Complaints.Where(c => c.ComplaintCode == complaintCloseDto.ComplaintCode).FirstOrDefaultAsync();

                if (complaint is null) return TypedResults.NotFound();

                complaint.StatusCode = "00003";
              
                await db.SaveChangesAsync();
            }

            else
            {
                return TypedResults.BadRequest("Closing the complaint failed.");
            }

            return TypedResults.Created($"/complaint-close/{complaintClose.ComplaintCloseId}", complaintCloseDto);

        }

        // get the closing details for a complaint
        public static async Task<IResult> GetComplaintClose(string complaintCode, GsdsDb db)
        {
            var allData = await db.ComplaintClose
                .Where(_ => _.ComplaintCode == complaintCode)
                .ToListAsync();

            return TypedResults.Ok(allData);
        }
    }
}
