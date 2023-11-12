using Gsds.Data;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.Dossier;
using GsdsV2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintMemoController : ControllerBase
    {
        public static async Task<IResult> AddComplaintMemo(string complaintCode, ComplaintMemoDto dto, ClaimsPrincipal user, GsdsDb db)
        {
            var complaint  = await db.Complaints.Where(c => c.ComplaintCode == complaintCode).FirstOrDefaultAsync();

            if (complaint != null)
            {
                var allData = await db.ComplaintMemos.ToArrayAsync();

                var complaintMemo = new ComplaintMemo();
                complaintMemo.Id = allData.Last().Id + 1;
                complaintMemo.ComplaintCode = complaintCode;
                complaintMemo.UsernameFrom = user.FindFirstValue(ClaimTypes.GivenName);
                complaintMemo.UsernameTo = dto.UsernameTo;
                complaintMemo.Title = dto.Title;
                complaintMemo.Details = dto.Details;

                db.ComplaintMemos.Add(complaintMemo);
                await db.SaveChangesAsync();

                return TypedResults.Created($"/complaint-memo/{complaintMemo.Id}", complaintMemo);
            }
            else{
                return TypedResults.BadRequest("Complaint with such code is not found");
            }
        }


        public static async Task<IResult> GetAllComplaintMemos(string complaintCode, GsdsDb db)
        {
            var complaint = await db.Complaints.Where(c => c.ComplaintCode == complaintCode).FirstOrDefaultAsync();

            if(complaint != null)
            {
                return TypedResults.Ok(
                    await db.ComplaintMemos.Where(_ => _.ComplaintCode == complaintCode)
                    .Select(cm => new
                    {
                        cm.Id,
                        cm.ComplaintCode,
                        cm.DueDate,
                        cm.Status,
                        cm.UsernameFrom,
                        cm.UsernameTo,
                        cm.Title,
                        cm.Details
                    }).ToListAsync()); 
            }

            else
            {
                return TypedResults.BadRequest("Complaint with such code not found");
            }
        }

    }
}
