using Gsds.Data;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.Dossier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintAdditionalDataController : ControllerBase
    {
        // get complaint's additional data
        public static async Task<IResult> GetComplaintAdditionalData(string complaintCode, GsdsDb db)
        {
            var additionalData = await db.ComplaintAdditionalData
                .Where(_ => _.ComplaintCode == complaintCode)
                .Select((x) => new ComplaintAdditionalDataDto(x))
                .ToArrayAsync();

            if(additionalData.Count() > 0 )
            {
                return TypedResults.Ok(additionalData);
            }

            else
            {
                return TypedResults.NotFound("No data found");
            }
        }

        // Add more data to a complaint
        public static async Task<IResult> CreateComplaintAdditionalData(ComplaintAdditionalDataDto additionalDataDto, GsdsDb db)
        {
            var allData = await db.ComplaintAdditionalData.ToArrayAsync();

            var cmpltAdditionalData = new ComplaintAdditionalData();
            cmpltAdditionalData.RefCode = allData.Last().RefCode + 1;
            cmpltAdditionalData.ComplaintCode = additionalDataDto.complaintCode;
            cmpltAdditionalData.Title = additionalDataDto.Title;
            cmpltAdditionalData.Comment = additionalDataDto.Comment;
            
            db.ComplaintAdditionalData.Add(cmpltAdditionalData);
            
            if(await db.SaveChangesAsync() > 0)
            {
                additionalDataDto = new ComplaintAdditionalDataDto(cmpltAdditionalData);
                return TypedResults.Created($"/api/complait-additional-data/{cmpltAdditionalData.RefCode}", additionalDataDto);
            }
            else
            {
                return TypedResults.BadRequest("Something bad happened.");
            }
        }

    }
}
