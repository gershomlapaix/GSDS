using GsdsAuth.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Users
{
    [Table("DEPARTMENT", Schema ="ADMIN")]
    public class Department
    {
        [Column("DEPARTMENT_ID")]
        public string Id { get; set; }

        [Column("DEPARTMENT_NAME")]
        public string DepartmentName { get; set; }

        [Column("DESCRIPTION")]
        public string description { get; set; }

        // Navigation properties
        public List<User>? Users { get; set; }
    }
}
