using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("INSTITUTION", Schema ="ADMIN")]
    public class Institution
    {
        [Column("INSTITUTION_ID")]
        public double Id { get; set; }

        [Column("INSTITUTION_NAME")]
        public string InstitutionName { get; set; }

        [Column("ID_ROLE")]
        public string? RoleId { get; set;}

        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("TRANS_DATE")]
        public DateTime TransDate { get; set; }

        [Column("USERNAME")]
        public string? Username { get; set; }

        [Column("PASSWORD")]
        public string? Password { get; set; }

        [Column("EMAIL")]
        public string? Email { get; set; }

        // Navigation properties
        public List<Complaint>? Complaints { get; set; }
        public List<InstitutionComplaint>? InstitutionsCompls { get; set; }
    }
}
