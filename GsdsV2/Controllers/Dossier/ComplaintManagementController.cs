using Gsds.Data;
using GsdsV2.DTO;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.Dossier;
using GsdsV2.Models.HelperModels;
using GsdsV2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintManagementController : ControllerBase
    {
        // Forwarding a complaint
        public static async Task<IResult> forwardingComplaint(string complaintCode, ComplaintManagementDto cmpltMngDto, ClaimsPrincipal user, IEmailService emailService, GsdsDb db)
        {

            var complaintManagement = new ComplaintManagement();
            complaintManagement.SeqNumber = cmpltMngDto.SeqNumber;
            complaintManagement.TransDate = DateTime.Now;
            complaintManagement.ComplaintCode = complaintCode;
            complaintManagement.Username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            complaintManagement.LevelFrom = user.FindFirstValue(ClaimTypes.Role);
            complaintManagement.LevelTo = cmpltMngDto.LevelTo;
            complaintManagement.DueDate = DateTime.Now.AddDays(cmpltMngDto.DueDate);
            complaintManagement.InternalComment = cmpltMngDto.InternalComment;
            complaintManagement.ExternalComment = cmpltMngDto.ExternalComment;
            complaintManagement.Cc = cmpltMngDto.CcRole; // TO BE USED LATER

            db.ComplaintManagements.Add(complaintManagement);

            if (await db.SaveChangesAsync() > 0)
            {
                var newComplaintRole = new ComplaintRoles();
                newComplaintRole.ComplaintCode = complaintCode;
                newComplaintRole.RoleId = cmpltMngDto.LevelTo;

                db.ComplaintRoles.Add(newComplaintRole);
                await db.SaveChangesAsync();

                // sending emails

                TimeSpan start = new TimeSpan(24, 0, 0); //10 o'clock
                TimeSpan end = new TimeSpan(12, 0, 0); //12 o'clock
                TimeSpan now = DateTime.Now.TimeOfDay;


                var emailRequest = new EmailDto();
                emailRequest.From = user.FindFirstValue(ClaimTypes.Email);
                emailRequest.To = "department@gmail.com";
                emailRequest.Subject = "New Assignment";
                emailRequest.Body = $"{((now > start) && (now < end) ? "Good morning" : "Good Afternoon")}, \n This is a new case assigned to your department. " +
                    $"Work on it in a given time. In any case you encounter any issue, do not hesitate to ask. \n" +
                    $"Thank you.";

                emailService.SendEmailAsync(emailRequest);
                return TypedResults.Ok("Done");
            }

            else
            {
                return TypedResults.BadRequest("There have been a problem.");
            }
        }
    }
}
