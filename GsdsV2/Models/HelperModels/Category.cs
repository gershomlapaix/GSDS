using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("COMPLAINT_CATEGORY", Schema = "ADMIN")]
    public class Category
    {
        [Key]
        [Column("ID_CATEGORY")]
        public string CategoryId { get; set; }

        [Column("CATEGORY")]
        public string CategoryName { get; set; }


        [Column("DESCRIPTION")]
        public string Description { get; set; }


        [Column("ID_COMPLAINT_TYPE")]
        public string ComplaintTypeId { get; set; }

        // NAVIGATION PROPERTIES
        public List<Complaint>? Complaints { get; set; }
    }
}
