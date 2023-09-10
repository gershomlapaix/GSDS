using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("PROVINCE", Schema ="ADMIN")]
    public class Province
    {
        [Column("PROVINCE_ID")]
        public char Id { get; set; }

        [Column("PROVINCE_NAME")]
        public string ProvinceName { get; set; }

        [Column("PROVINCEENGLISH")]
        public string ProvinceEnglish { get; set; }
    }
}
