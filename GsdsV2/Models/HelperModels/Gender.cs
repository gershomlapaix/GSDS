using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("GENDER", Schema = "ADMIN")]
    public class Gender
    {
        [Column("ID_GENDER")]
        public char Id { get; set; }

        [Column("GENDER")]
        public string Name { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
