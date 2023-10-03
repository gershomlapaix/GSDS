using GsdsV2.Models.HelperModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.DTO.HelperDtos
{
    public class ProvinceDto
    {
        public string Id { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceEnglish { get; set; }

        public ProvinceDto() { }
        public ProvinceDto(Province province) =>
            (Id, ProvinceName, ProvinceName) = (province.Id, province.ProvinceName, province.ProvinceEnglish);
    }
}
