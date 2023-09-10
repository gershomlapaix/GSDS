using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("SECTOR", Schema ="ADMIN")]
    public class Sector
    {
        [Column("SECTOR_ID")]
        public char Id { get; set; }

        [Column("DISTRICT_ID")]
        public char DistrictId { get; set; }

        [Column("SECTOR_NAME")]
        public string SectorName { get; set;}
    }
}
