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
            var newACcused = new Accused();
                newACcused.complaintCode = accusedDto.complaintCode;
                newACcused.PeronTypeId = accusedDto.personTypeId;
                newACcused.IdNumber = accusedDto.IdNumber;
                newACcused.IdType = accusedDto.IdType;
                newACcused.GenderId = accusedDto.GenderId;
                newACcused.IdDetails = accusedDto.IdDetails;
                newACcused.Names = accusedDto.Names;
                newACcused.birthDate = accusedDto.birthDate;
                newACcused.MaritalStatusId = accusedDto.MaritalStatusId;
                newACcused.ProvinceId = accusedDto.ProvinceId;
                newACcused.DistrictId = accusedDto.DistrictId;
                newACcused.SectorId = accusedDto.SectorId;
                newACcused.CellId = accusedDto.CellId;
                newACcused.Phone = accusedDto.Phone;
                newACcused.RegistrationDate = DateTime.Now;
                newACcused.complainerId = accusedDto.complainerId;
            

            db.Accuseds.Add(newACcused);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/complainer/{accusedDto.IdNumber}", accusedDto);
        }

        // Get all accuseds
        public static async Task<IResult> getAllAccuseds(GsdsDb db)
        {
            return TypedResults.Ok(await db.Accuseds.ToArrayAsync());

        }

        // Get one accused
        public static async Task<IResult> getAccusedByComplaintCode(string complaintCode, GsdsDb db)
        {
            var accused = await db.Accuseds.Where(_ => _.complaintCode == complaintCode).ToListAsync();
            return TypedResults.Ok(accused[0]);

        }
    }
}
