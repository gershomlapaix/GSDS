namespace GsdsV2.DTO.Dossier
{
    public class ComplainerDto
    {
        public string personTypeId { get; set; }

        public string IdType { get; set; }

        public string IdNumber { get; set; }

        public string GenderId { get; set; }

        public string IdDetails { get; set; }

        public DateTime birthDate { get; set; }
        public string MaritalStatusId { get; set; }

        // LOCATION
        public string ProvinceId { get; set; }

        public string DistrictId { get; set; }

        public string SectorId { get; set; }

        public string CellId { get; set; }

        public string Phone { get; set; }

        public string PoBox { get; set; }

        // if deputy's around
        public decimal IsDeputy { get; set; }

        public string DeputyNames { get; set; }

        public string DeputyPhone { get; set; }

        public string DeputyEmail { get; set; }
    }
}
