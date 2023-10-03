using System.Security.Claims;
using Gsds.Data;
using Gsds.Models.Dossier;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.Dossier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gsds.Controllers.Dossier
{
    public class ComplainerController : ControllerBase
    {
        public static async Task<IResult> RegisterComplainer(ComplainerDto complainerDto, ClaimsPrincipal user, GsdsDb db)
        {
            var newComplainer = new Complainer()
            {
                PeronTypeId = complainerDto.personTypeId,
                Id = complainerDto.IdNumber,
                IdType = complainerDto.IdType,
                GenderId = complainerDto.GenderId,
                IdDetails = complainerDto.IdDetails,
                TheNames = user.FindFirst(ClaimTypes.GivenName).Value,
                birthDate = complainerDto.birthDate,
                MaritalStatusId = complainerDto.MaritalStatusId,
                ProvinceId = complainerDto.ProvinceId,
                DistrictId = complainerDto.DistrictId,
                SectorId = complainerDto.SectorId,
                CellId = complainerDto.CellId,
                Phone = complainerDto.Phone,
                Email = user.FindFirst(ClaimTypes.Email).Value,
                Username = user.FindFirst(ClaimTypes.NameIdentifier).Value,
                FirstName = user.FindFirst(ClaimTypes.GivenName).Value.Split(' ')[0],
                LastName = user.FindFirst(ClaimTypes.GivenName).Value.Split(' ')[1],
                RegistrationDate = DateTime.Now
            };

            db.Complainers.Add(newComplainer);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/complainer/{complainerDto.IdNumber}", complainerDto);
        }

        // get all complaints
        public static async Task<IResult> GetAllComplainers(GsdsDb db)
        {
            //var complainers = await db.Complainers
            //    .Include(_ => _.Complaints).ToListAsync();
            var complainers = await db.Complainers.ToListAsync();
            return TypedResults.Ok(complainers);
        }

        //Get One complainer
        public static async Task<IResult> GetComplainerById(string complainerNId, GsdsDb db)
        {
            //return await db.Complainers.Where(c => c.Id == complainerNId).Include(_ => _.Complaints).ToListAsync()
            return await db.Complainers.Where(c => c.Id == complainerNId).ToListAsync()
               is List<Complainer> theComplainer
           ? TypedResults.Ok(theComplainer[0])
           : TypedResults.NotFound();

        }

        // get complaints by the username
        public static async Task<IResult> GetMyDetailsByUsername(ClaimsPrincipal user, GsdsDb db)
        {
            var complainer = await db.Complainers.Where(c => c.Username == user.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();

            if(complainer.Count() != 0) {
                return TypedResults.Ok(complainer[0]);
            }
            else
            {              
                return TypedResults.NotFound(new { requiredUpdate = true });
            }
        }

        // get my uploaded files
        public static async Task<IResult> GetMyUploadedFiles(ClaimsPrincipal user, GsdsDb db)
        {
            var files = await db.Attachments.Where(_ => _.UploadedBy == user.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();

            if (files.Count() > 0)
            {
                return TypedResults.Ok(files);
            }

            else
            {
                return TypedResults.NotFound("You do not have files yet.");
            }
        }
    }
}