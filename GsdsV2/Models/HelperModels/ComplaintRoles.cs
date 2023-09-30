using GsdsV2.Models.Dossier;
using Microsoft.Identity.Client.Extensions.Msal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace GsdsV2.Models.HelperModels
{
    [Table("COMPLAINT_ROLES", Schema ="ADMIN")]
    public class ComplaintRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("COMPLAINT_CODE")]
        public string? ComplaintCode { get; set; }

        [Column("ID_ROLE")]
        public string? RoleId { get; set; }

        // Navigation properties
        public Complaint? Complaint { get; set; }
        public ManagerRoles? ManagerRoles { get; set; }
    }
}
