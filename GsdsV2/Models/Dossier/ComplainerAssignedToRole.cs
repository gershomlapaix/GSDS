using Gsds.Models.Dossier;
using GsdsV2.Models.HelperModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Dossier
{
    [Table("COMPLAINER_ASSIGNED", Schema = "ADMIN")]
    public class ComplainerAssignedToRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("COMPLAINER_ID")]
        public string? ComplainerId { get; set; }

        [Column("ID_ROLE")]
        public string? RoleId { get; set; }

        // Navigation properties
        public Complainer? Complainer { get; set; }
        public ManagerRoles? ManagerRoles { get; set; }
    }
}
