using GsdsV2.Models.HelperModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Users
{
    [Table("UserSection")]
    public class UserSection
    {
        [Column("USERSECTIONID")]
        public double Id { get; set; }

        [Column("SECTION_NAME")]
        public string SectionName { get; set; }
    }
}
