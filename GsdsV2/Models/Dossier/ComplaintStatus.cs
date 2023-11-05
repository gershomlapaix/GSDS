using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Dossier
{
    [Table("COMPLAINT_STATUS", Schema ="ADMIN")]
    public class ComplaintStatus
    {
        [Column("STATUS_CODE")]
        public string Id { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("STATUS_KINY")]
        public string StatusKiny { get; set; }

        // Navigation properties
        public List<Complaint>? Complaints { get; set; }
    }
}
