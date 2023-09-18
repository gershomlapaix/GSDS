using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("DISTRICT", Schema ="ADMIN")]
    public class District
    {
        [Column("DISTRICT_ID")]
        public string Id { get; set; }

        [Column("POVINCE_ID")]
        public string ProvinceId { get; set; }

        [Column("DISTRICT_NAME")]
        public string DistrictName { get; set; }
    }
}
