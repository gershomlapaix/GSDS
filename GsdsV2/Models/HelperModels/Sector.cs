using Gsds.Models.Dossier;
using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("SECTOR", Schema ="ADMIN")]
    public class Sector
    {
        [Column("SECTOR_ID")]
        public string Id { get; set; }

        [Column("DISTRICT_ID")]
        public string DistrictId { get; set; }

        [Column("SECTOR_NAME")]
        public string SectorName { get; set;}

        public District District { get; set; }
        public List<Cell> Cells { get; set; }
        public List<Complaint>? Complaints { get; set; }
        public List<Complainer>? Complainers { get; set;}
        public List<Accused>? Accuseds { get; set; }
    }
}
