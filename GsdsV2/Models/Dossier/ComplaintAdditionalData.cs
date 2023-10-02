using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Dossier
{
    [Table("USER_FEEDBACK", Schema ="ADMIN")]
    public class ComplaintAdditionalData
    {
        [Key]
        [Column("REF_CODE")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Double RefCode { get; set; } 

        [Column("COMPLAINT_CODE")]
        public string? ComplaintCode { get; set; }

        [Column("TRANS_DATE")]
        public DateTime TransferDate = DateTime.Now;

        [Column("ATTACHMENT")]
        public string? TheAttachments { get; set; } = null;

        [Column("Title")]
        public string? Title { get; set; }

        [Column("COMMENTS")]
        public string? Comment { get; set; }


        // Navigation properties
        public Complaint? Complaint { get; set; }
    }
}
