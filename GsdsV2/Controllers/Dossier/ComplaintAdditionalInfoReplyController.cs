using Gsds.Data;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.Dossier;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.Dossier
{
    public class ComplaintAdditionalInfoReplyController
    {

        // get all the replies on the submitted additional data
        public static async Task<IResult> GetComplaintAdditionalDataReplyByComplaintCode(string complaintCode, GsdsDb db)
        {
            var additionalDataReply = await db.ComplaintAdditionalInfoReplies
                .Where(_ => _.ComplaintCode == complaintCode)
                .Select((x) => new {
                    x.RefCode,
                    x.ComplaintCode,
                    x.Title,
                    x.Comment,
                    x.TransferDate
                })
                .ToArrayAsync();

            if (additionalDataReply.Count() > 0)
            {
                return TypedResults.Ok(additionalDataReply);
            }

            else
            {
                return TypedResults.NotFound("No data found");
            }
        }

        public static async Task<IResult> GetComplaintAdditionalDataReplyByRefCode(Double refCode, GsdsDb db)
        {
            var additionalDataReply = await db.ComplaintAdditionalInfoReplies
                .Where(_ => _.RefCode == refCode)
                .Select((x) => new {
                    x.RefCode,
                    x.ComplaintCode,
                    x.Title,
                    x.Comment,
                    x.TransferDate
                })
                .ToArrayAsync();

            if (additionalDataReply.Count() > 0)
            {
                return TypedResults.Ok(additionalDataReply);
            }

            else
            {
                return TypedResults.NotFound("No data found");
            }
        }

        // create a reply on the user's additional data submitted
        public static async Task<IResult> CreateComplaintAdditionalInfoReply(ComplaintAdditionalInfoReplyDto complaintAdditionalInfoDto, GsdsDb db)
        {
            var allData = await db.ComplaintAdditionalInfoReplies.ToListAsync();

            var cmpltReply = new ComplaintAdditionalInfoReply();
            cmpltReply.RefCode = complaintAdditionalInfoDto.RefCode;
            cmpltReply.ComplaintCode = complaintAdditionalInfoDto.ComplaintCode;
            cmpltReply.Title = complaintAdditionalInfoDto.Title;
            cmpltReply.Comment = complaintAdditionalInfoDto.Comment;
            cmpltReply.ComplaintReplyId = allData.Count() + 1;

            db.ComplaintAdditionalInfoReplies.Add(cmpltReply);

            if (await db.SaveChangesAsync() > 0)
            {
                return TypedResults.Created($"/api/complait-additional-data/{cmpltReply.RefCode}", complaintAdditionalInfoDto);
            }
            else
            {
                return TypedResults.BadRequest("Something bad happened.");
            }
        }
    }
}
