using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.DTO.Dossier
{
    public class ComplaintCloseDto
    {
        public string? ComplaintCode { get; set; }

        public string? ClosingReason { get; set; }

        public string? TransferDate { get; set; }
    }
}
