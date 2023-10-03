using Gsds.Data;
using Gsds.Models.Dossier;
using GsdsAuth.Models;
using GsdsV2.DTO;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.Dossier;
using GsdsV2.Models.HelperModels;
using GsdsV2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintController : ControllerBase
    {
        // Make a new complaint
        public static async Task<IResult> createComplaint(ComplaintDto complaint, ClaimsPrincipal user, IEmailService emailService, GsdsDb db)
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
                StartOffice = "Reception",
                ComplaintCategoryId = complaint.ComplaintCategoryId,
                PriorityId = "00003",
                RoleId = complaint.RoleId
            };

            var newComplaintRole = new ComplaintRoles();
            newComplaintRole.ComplaintCode = complaint.ComplaintCode;
            newComplaintRole.RoleId = complaint.RoleId;

            db.Complaints.Add(newComplaint);
            if (await db.SaveChangesAsync() > 0)
            {
                db.ComplaintRoles.Add(newComplaintRole);
                await db.SaveChangesAsync();
            }

            TimeSpan start = new TimeSpan(24, 0, 0);
            TimeSpan end = new TimeSpan(12, 0, 0); //12 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;


            var emailRequest = new EmailDto();
            emailRequest.From = "admin@omb.com";
            emailRequest.To = user.FindFirstValue(ClaimTypes.Email);
            emailRequest.Subject = "Kumenyesha";
            emailRequest.Body = $"{((now > start) && (now < end) ? "Mwaramutse" : "Mwiriwe")}, \n Ikirego cyawe kirakiriwe. " +
                $"Turagusaba kuguma gukoresha iyi System kugira ngo umenye aho kigeze gikorwaho.\n" +
                $"Mugihe waba ufite andi makuru yakunganira ikirego cyawe, turagusaba ko wajya uhita uyashyira muri system.\n" +
                $"Murakoze,";

            emailService.SendEmailAsync(emailRequest);

            return TypedResults.Created($"/complainer/{complaint.ComplaintCode}", complaint);
        }


        //Get all complaints
        public static async Task<IResult> getAllComplaints(GsdsDb db)
        {
            return TypedResults.Ok(
                await db.Complaints
                .Select(c => new {
                    c.ComplaintCode,
                    c.ComplainerId,
                    c.AccusedIdNumber,
                    c.Subject,
                    c.complaintDescription,
                    c.ComplaintOwner,
                    c.Accused.Names,
                    c.Complainer.TheNames,
                    c.Province.ProvinceName,
                    c.District.DistrictName,
                    c.Sector.SectorName,
                    c.Cell.CellName
                })
                .ToListAsync());
        }

        //Get One complaints
        public static async Task<IResult> getOneComplaint(string complaintCode, GsdsDb db)
        {
            return TypedResults.Ok(await db.Complaints
                .Where(c => c.ComplaintCode == complaintCode)
                .Include(c => c.Province)
                .ToListAsync());
        }

        //Get complaint by categoru
        public static async Task<IResult> getComplaintByCategory(string cmpltCategory, GsdsDb db)
        {
            return TypedResults.Ok(await db.Complaints
                .Where(c => c.ComplaintCategoryId == cmpltCategory)
                .ToListAsync());
        }


        // Get the files
        public static async Task<IResult> getComplaintFiles(string complaintCode, GsdsDb db)
        {
            return TypedResults.Ok(await db.Complaints
                 .Where(c => c.ComplaintCode == complaintCode)
                 .Include(c => c.ComplaintAttachments)
                 .ToListAsync());
        }

        // Get current user complaints
        public static async Task<IResult> getLoggedInUserComplaints(ClaimsPrincipal user, GsdsDb db)
        {
            var complainer = await db.Complainers.Where(_ => _.Username == user.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();
            if(complainer.Count() > 0)
            {
                return TypedResults.Ok(await db.Complaints
                 .Where(c => c.ComplainerId == complainer[0].Id)
                 .Include(a => a.Accused)
                 .ToListAsync());
            }
            return TypedResults.NotFound("Either the user has not fully registered or is not found.");
        }


        // Get the roles
        public static async Task<IResult> getComplaintRoles(string complaintCode, GsdsDb db)
        {
            return TypedResults.Ok(await db.Complaints
                 .Where(c => c.ComplaintCode == complaintCode)
                 .Include(c => c.Roles)
                 .ToListAsync());
        }
    }
}
