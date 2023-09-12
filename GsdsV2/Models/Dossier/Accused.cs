// Accused model
using Gsds.Constants.Dossier;
using GsdsV2.Models.Dossier;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gsds.Models.Dossier{

    [Table("ACCUSED_TEMP", Schema ="ADMIN")]
    public class Accused{
        [Key]
        [Column("COMPLAINT_CODE")]
        public string complaintCode { get; set; }

        [Column("ID_PERSON_TYPE")]
        public string? PeronTypeId { get; set; }

        [Column("NAMES")]
        public string? Names { get; set; }

        [Column("SEX")]
        public string? GenderId { get; set; }

        [Column("ID_TYPE")]
        public string? IdType { get; set; }

        [Column("ID_NUMBER")]
        public string? IdNumber { get; set; }

        [Column("ID_DETAILS")]
        public string? IdDetails { get; set; }

        [Column("DOB")]
        public DateTime birthDate { get; set; }

        [Column("DATE_CREATION")]
        public DateTime RegistrationDate { get; set; }

        [Column("CREATED_BY")]
        public string? CreatedBy { get; set; }

        [Column("ID_MARITAL_STATUS")]
        public string? MaritalStatusId { get; set; }

        [Column("TELEPHONE")]
        public string? Phone { get; set; } = string.Empty;

        [Column("EMAIL")]
        public string? Email { get; set; } = string.Empty;

        [Column("PO_BOX")]
        public string? PoBox { get; set; } = string.Empty;

        // LOCATION
        [Column("PROVINCE_ID")]
        public string? ProvinceId { get; set; }

        [Column("DISTRICT_ID")]
        public string? DistrictId { get; set; }

        [Column("SECTOR_ID")]
        public string? SectorId { get; set; }

        [Column("CELL_ID")]
        public string? CellId { get; set; }

        [Column("IDPLANTIFF")]
        public string complainerId { get; set; }


        // NAVIGATION PROPERTIES
        public Complaint Complaint { get; set; }
    }
}