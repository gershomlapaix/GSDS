using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Controllers.Dossier
{
    [Table("USER_FEEDBACK_REPLY", Schema ="ADMIN")]
    public class ComplaintAdditionalInfoReply
    {
        [Column("CMPLT_REPLYID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ComplaintReplyId { get; set; }

        [Column("REF_CODE")]
        public Double RefCode { get; set; }  // foreign key

        [Column("COMPLAINT_CODE")]
        public string? ComplaintCode { get; set; }

        [Column("TRANS_DATE")]
        public DateTime TransferDate { get; set; } = DateTime.Now;

        [Column("COMMENTS")]
        public string? Comment { get; set; } = string.Empty;

        [Column("ATTACHMENT")]
        public string? TheAttachments { get; set; } = null;

        [Column("TITLE")]
        public string? Title { get; set; }

        [Column("EXTERNAL")]
        public string? External { get; set; } = null;
    }
}
