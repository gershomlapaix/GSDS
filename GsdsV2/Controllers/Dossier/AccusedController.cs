using Gsds.Data;
using Gsds.Models.Dossier;
using GsdsV2.DTO.Dossier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GsdsV2.Controllers.Dossier
{
    public class AccusedController : ControllerBase
    {

        //Register a new accused person

        public static async Task<IResult> RegisterAccused(AccusedDto accusedDto, GsdsDb db)
        {
            var newACcused = new Accused()
            {
                complaintCode = accusedDto.complaintCode,
                PeronTypeId = accusedDto.personTypeId,
                IdNumber = accusedDto.IdNumber,
                IdType = accusedDto.IdType,
                GenderId = accusedDto.GenderId,
                IdDetails = accusedDto.IdDetails,
                Names = accusedDto.Names,
                birthDate = accusedDto.birthDate,
                MaritalStatusId = accusedDto.MaritalStatusId,
                ProvinceId = accusedDto.ProvinceId,
                DistrictId = accusedDto.DistrictId,
                SectorId = accusedDto.SectorId,
                CellId = accusedDto.CellId,
                Phone = accusedDto.Phone,
                RegistrationDate = DateTime.Now,
                complainerId = accusedDto.complainerId,
            };

            db.Accuseds.Add(newACcused);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/complainer/{accusedDto.IdNumber}", accusedDto);
        }

        // Get all accuseds
        public static async Task<IResult> getAllAccuseds(GsdsDb db)
        {
            return TypedResults.Ok(await db.Accuseds.ToArrayAsync());

        }
    }
}
