using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("COUNTRYLIST", Schema = "ADMIN")]
    public class Country
    {
        [Column("COUNTRYID")]
        public string Id { get; set; }

        [Column("COUNTRYNAME")]
        public string? CountryName { get; set; }

        [Column("COUNTRYCODE")]
        public string? CountryCode { get; set; }

        [Column("PHONECODE")]
        public string? PhoneCode { get; set; }

    }
}
