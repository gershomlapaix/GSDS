using System.Security.Claims;
using Gsds.Data;
using Gsds.Models.Dossier;
using GsdsV2.DTO.Dossier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gsds.Controllers.Dossier
{
    public class ComplainerController : ControllerBase
    {

        // register a complainer method

        public static async Task<IResult> RegisterComplainer(ComplainerDto complainerDto, ClaimsPrincipal user, GsdsDb db)
        {

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

        public static async Task<IResult> GetAllComplainers(GsdsDb db)
        {
            return TypedResults.Ok(await db.Complainers.ToArrayAsync());
        }
    }
}