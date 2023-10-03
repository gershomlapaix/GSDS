using Gsds.Models.Dossier;
using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GsdsV2.Models.HelperModels
{
    [Table("PROVINCE", Schema ="ADMIN")]
    public class Province
    {
        [Column("PROVINCE_ID")]
        public string Id { get; set; }

        [Column("PROVINCE_NAME")]
        public string ProvinceName { get; set; }

        [Column("PROVINCEENGLISH")]
        public string ProvinceEnglish { get; set; }

        [JsonIgnore]
        public List<Complaint>? Complaints { get; set; }    // complaints

        //[JsonIgnore]
        public List<District>? Districts { get; set; }   // corresponding districts

        public List<Complainer>? Complainers { get; set; }      // complainers
        public List<Accused>? Accuseds { get; set; }        // accuseds
    }
}
