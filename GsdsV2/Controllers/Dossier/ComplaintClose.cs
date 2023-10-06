using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Controllers.Dossier
{
    [Table("COMPLAINT_CLOSE", Schema = "ADMIN")]
    public class ComplaintClose
    {
        [Column("COMPLAINTCLOSEID")]
        public int? ComplaintCloseId { get; set; }

        [Column("COMPLAINT_CODE")]
        public string? ComplaintCode { get; set; }

        [Column("COMPLAINTCLOSEREASON")]
        public string? ClosingReason { get; set; }

        [Column("TransDate")]
        public DateTime? TransferDate { get; set; } = DateTime.Now;
    }
}
