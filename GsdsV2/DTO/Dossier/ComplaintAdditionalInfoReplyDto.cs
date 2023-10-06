using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GsdsV2.DTO.Dossier
{
    public class ComplaintAdditionalInfoReplyDto
    {
        public Double RefCode { get; set; }  // foreign key
        public string? ComplaintCode { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
    }
}
