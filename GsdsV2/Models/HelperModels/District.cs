using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("DISTRICT", Schema ="ADMIN")]
    public class District
    {
        [Column("DISTRICT_ID")]
        public char Id { get; set; }

        [Column("POVINCE_ID")]
        public char ProvinceId { get; set; }

        [Column("DISTRICT_NAME")]
        public string DistrictName { get; set; }
    }
}
