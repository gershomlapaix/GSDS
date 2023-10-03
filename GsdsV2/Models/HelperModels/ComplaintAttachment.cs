using Gsds.Models.Dossier;
using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("ATTACHMENT", Schema ="ADMIN")]
    public class ComplaintAttachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("COMPLAINT_CODE")]
        [ForeignKey("COMPLAINT_CODE")]
        public string? ComplaintCode { get; set; }

        [Column("UPLOADED_BY")]
        public string? UploadedBy { get; set; }

        [Column("ATTACHMENT")]
        public string? FilePath { get; set; }

        [Column("FILENAME")]
        public string? FileName { get; set; }

        //[Column("DESCRIPTION")]
        //public string? Description { get; set; }

        public Complaint? Complaint { get; set; }
    }
}
