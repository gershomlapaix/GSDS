using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("ATTACHMENT_DESCRIPTION", Schema = "ADMIN")]
    public class AttachmentDescription
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }

        [Column("DESCRIPTION")]
        public string? Description { get; set; }


        // navigation properties
        public List<ComplaintAttachment>? ComplaintAttachments { get; set;}
    }
}
