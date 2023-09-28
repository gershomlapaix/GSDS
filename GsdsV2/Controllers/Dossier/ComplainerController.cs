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
            Console.WriteLine("the user" + user.FindFirstValue(ClaimTypes.Email));

            var newComplainer = new Complainer()
            {
                PeronTypeId = complainerDto.personTypeId,
                Id = complainerDto.IdNumber,
                IdType = complainerDto.IdType,
                GenderId = complainerDto.GenderId,
                IdDetails = complainerDto.IdDetails,
                Names = user.FindFirst(ClaimTypes.GivenName).Value,
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
        public static async Task<IResult> getSingleComplainer(string complainerNId, GsdsDb db)
        {
            //return await db.Complainers.Where(c => c.Id == complainerNId).Include(_ => _.Complaints).ToListAsync()
            return await db.Complainers.Where(c => c.Id == complainerNId).ToListAsync()
               is List<Complainer> theComplainer
           ? TypedResults.Ok(theComplainer[0])
           : TypedResults.NotFound();

        }
    }
}