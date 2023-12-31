﻿using Gsds.Data;
using GsdsV2.Controllers.Dossier.HelperControllers;
using GsdsV2.DTO;
using GsdsV2.DTO.Dossier;
using GsdsV2.DTO.HelperDtos;
using GsdsV2.Models.Dossier;
using GsdsV2.Models.HelperModels;
using GsdsV2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintManagementController : ControllerBase
    {
        // Forwarding a complaint
        public static async Task<IResult> ForwardingComplaint(string complaintCode, ComplaintManagementDto cmpltMngDto, ClaimsPrincipal user, IEmailService emailService, GsdsDb db)
        {

            var allData = await db.ComplaintManagements.ToArrayAsync();

            var complaintManagement = new ComplaintManagement();
            complaintManagement.SeqNumber = allData.Last().SeqNumber + 1;
            complaintManagement.TransDate = DateTime.Now;
            complaintManagement.ComplaintCode = complaintCode;
            complaintManagement.Username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            complaintManagement.LevelFrom = user.FindFirstValue(ClaimTypes.Role);
            complaintManagement.LevelTo = cmpltMngDto.LevelTo;
            complaintManagement.InstitutionId = cmpltMngDto.InstitutionId;
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

                // update the status
                var complaint = await db.Complaints.Where(c => c.ComplaintCode == complaintCode).FirstOrDefaultAsync();

                if (complaint is null) return TypedResults.NotFound();

                complaint.StatusCode = "00002";

                await db.SaveChangesAsync();

                // sending emails

                TimeSpan start = new TimeSpan(24, 0, 0); //10 o'clock
                TimeSpan end = new TimeSpan(12, 0, 0); //12 o'clock
                TimeSpan now = DateTime.Now.TimeOfDay;
                var emailRequest = new EmailDto();

                emailRequest.From = user.FindFirstValue(ClaimTypes.Email);
                emailRequest.To = "admin@gmail.com";
                emailRequest.Subject = "New Assignment";
                emailRequest.Body = $"Hi, <br>" +
                    $"This is a new case assigned to your department. " +
                    $"Work on it in a given time. In any case you encounter any issue, do not hesitate to ask. <br>" +
                    $"Thank you.";

                emailService.SendEmailAsync(emailRequest);


                return TypedResults.Ok("Done");
            }

            else
            {
                return TypedResults.BadRequest("Forwarding a complaint failed");
            }
        }

        // forward to an institution
        public static async Task<IResult> ForwardToInsitution(string instId, string complaintCode, InstitutionComplDto dto,
            ClaimsPrincipal user, GsdsDb db)
        {
            //var institution = await db.Institutions.
            var institutionCompl = new InstitutionComplaint();
            institutionCompl.InstitutionId = (double.Parse(instId));
            institutionCompl.complaintCode = complaintCode;
            institutionCompl.ForwardedBy = user.FindFirstValue(ClaimTypes.GivenName);
            institutionCompl.FromEmail = user.FindFirstValue(ClaimTypes.Email);
            institutionCompl.Description = dto.Description;
            institutionCompl.Subject = dto.Subject;

            db.InstitutionComplaints.Add(institutionCompl);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/todoitems/{institutionCompl.Id}", institutionCompl);
        }

        // forwarded complaints
        public static async Task<IResult> GetAllForwarded(GsdsDb db)
        {
            return TypedResults.Ok(
                await db.ComplaintManagements
                .Select(cm => new { 
                   cm.SeqNumber,
                   cm.ComplaintCode,
                   cm.Username,
                   cm.LevelFrom,
                   cm.LevelTo,
                   cm.InstitutionId,
                   cm.TransDate,
                   cm.DueDate,
                   cm.InternalComment,
                   cm.ExternalComment,
                   cm.Cc
                })
                .ToArrayAsync());
        }

        // get forwards for a certain complaint
        public static async Task<IResult> GetForwardedByComplaintCode(string complaintCode, GsdsDb db)
        {
            return TypedResults.Ok(
                await db.ComplaintManagements
                .Where(_ => _.ComplaintCode == complaintCode)
                .Select(cm => new {
                    cm.SeqNumber,
                    cm.ComplaintCode,
                    cm.Username,
                    cm.LevelFrom,
                    cm.LevelTo,
                    cm.InstitutionId,
                    cm.TransDate,
                    cm.DueDate,
                    cm.InternalComment,
                    cm.ExternalComment,
                    cm.Cc
                })
                .ToListAsync());
        }
    }
}
