namespace GsdsV2.DTO.Dossier
{
    public class Complainer
    {
        public string Id { get; set; }

        public char personTypeId { get; set; }

        public char GenderId { get; set; }

        public string IdNumber { get; set; }

        public string IdDetails { get; set; }

        public DateTime birthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public char MaritalStatusId { get; set; }

        public string Phone { get; set; }

        public string PoBox { get; set; }

        // LOCATION
        public char ProvinceId { get; set; }

        public char DistrictId { get; set; }

        public char SectorId { get; set; }

        public char CellId { get; set; }

        public string Address { get; set; }
    }
}
