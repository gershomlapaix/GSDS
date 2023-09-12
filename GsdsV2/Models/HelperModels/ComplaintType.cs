using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("COMPLAINT_TYPE", Schema ="ADMIN")]
    public class ComplaintType
    {
        [Key]
        [Column("ID_COMPLAINT_TYPE")]
        public string complaintTypeid { get; set; }

        [Column("COMPLAINT_TYPE_NAME")]
        public string complaintTypename { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

    }
}
