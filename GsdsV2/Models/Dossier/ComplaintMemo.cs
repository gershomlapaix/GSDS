using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Dossier
{
    [Table("CMPLT_MEMO", Schema ="ADMIN")]
    public class ComplaintMemo
    {
        [Column("CMPLT_MEMOID")]
        public double Id { get; set; }

        [Column("COMPLAINT_CODE")]
        public string? ComplaintCode { get; set; }

        [Column("USERNAMEFROM")]
        public string? UsernameFrom {  get; set; }

        [Column("USERNAMETO")]
        public string? UsernameTo { get; set; }

        [Column("TITLE")]
        public string? Title { get; set; }

        [Column("ATTACHMENT")]
        public string? Attachment { get; set; }

        [Column("DETAIL")]
        public string? Details { get; set; }

        [Column("STATUS")]
        public double? Status { get; set; } = 0;

        [Column("DUE_DATE")]
        public DateTime? DueDate { get; set; } = DateTime.Now;

        // Navigation properties
        public Complaint? Complaint { get; set; }
    }
}
