using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.DTO.Dossier
{
    public class ComplaintAdditionalDataDto
    {
        public string? complaintCode { get; set; }

        public string? Title { get; set; }

        public string? Comment { get; set; }

        public ComplaintAdditionalDataDto() { }
        public ComplaintAdditionalDataDto(ComplaintAdditionalData additionalData)
        {
            (complaintCode, Title, Comment) = (additionalData.ComplaintCode ,additionalData.Title, additionalData.Comment);
        }
    }
}
