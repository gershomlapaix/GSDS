using GsdsAuth.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GsdsV2.Models.Users
{

    [Table("UserGroup")]
    public class UserGroup
    {
        [Column("GroupID")]
        [Key]
        public int? GroupID { get; set; }

        [Column("GroupName")]
        public string? groupName { get; set; }

        // Navigation properties
        [JsonIgnore]
        public List<User>? Users { get; set; }
    }
}
