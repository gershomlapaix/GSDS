using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("MNGRS_ROLES", Schema ="ADMIN")]
    public class ManagerRoles
    {
        [Column("ID_ROLE")]
        public string Id { get; set; }

        [Column("ROLE_NAME")]
        public string RoleName { get; set; }

        [Column("ROLE_DESCRIPTION")]
        public string RoleDescription { get; set; }

        [Column("ID_ROLE_NEXT")]
        public string IdRoleNext { get; set; }

        // Navigation properties
        public List<Complaint>? Complaints { get; set; }
        public List<ComplaintRoles>? Roles { get; set; }
    }
}
