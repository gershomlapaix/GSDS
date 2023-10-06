using Gsds.Models.Dossier;
using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GsdsV2.Models.HelperModels
{
    [Table("DISTRICT", Schema ="ADMIN")]
    public class District
    {
        [Column("DISTRICT_ID")]
        public string Id { get; set; }

        [Column("POVINCE_ID")]
        public string ProvinceId { get; set; }
        [JsonIgnore]
        public Province Province { get; set; }

        [Column("DISTRICT_NAME")]
        public string DistrictName { get; set; }


        // Navigation properties
        public List<Sector> Sectors { get; set; }
        public List<Complaint>? Complaints { get; set; }
        public List<Complainer>? Complainers { get; set; }
        public List<Accused>? Accuseds { get; set; }
    }
}
