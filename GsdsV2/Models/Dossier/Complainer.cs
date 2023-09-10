// This class contains the details about the complainer
using System.ComponentModel.DataAnnotations.Schema;
using Gsds.Constants.Dossier;
using GsdsAuth.Models;

namespace Gsds.Models.Dossier{
    [Table("PLAINTIFF", Schema = "ADMIN")]
    public class Complainer {

        [Column("ID_NUMBER")]
        public string? Id { get; set; }

        [Column("ID_PERSON_TYPE")]
        public char? PeronTypeId { get; set; }

        [Column("SEX")]
        public char? GenderId { get; set; }

        [Column("ID_TYPE")]
        public char? IdType { get; set; }

        [Column("ID_DETAILS")]
        public string? IdDetails { get; set; }

        [Column("DOB")]
        public DateTime birthDate { get; set; }

        [Column("DATE_CREATION")]
        public DateTime RegistrationDate { get; set; }

        [Column("CREATED_BY")]
        public string? CreatedBy { get; set; }

        [Column("ID_MARITAL_STATUS")]
        public char? MaritalStatusId { get; set; } = '\0';

        [Column("TELEPHONE")]
        public string? Phone { get; set; } = string.Empty;

        [Column("EMAIL")]
        public string? Email { get; set; } = string.Empty;

        [Column("PO_BOX")]
        public string? PoBox { get; set; } = string.Empty;

        // LOCATION
        [Column("PROVINCE_ID")]
        public char? ProvinceId { get; set; } = '\0';

        [Column("DISTRICT_ID")]
        public char? DistrictId { get; set; } = '\0';

        [Column("SECTOR_ID")]
        public char? SectorId { get; set; } = '\0';

        [Column("CELL_ID")]
        public char? CellId { get; set; } = '\0';

        [Column("ADRESS")]
        public string? Address { get; set; } = string.Empty;

        // RELATED TO THE USER OBJECT

        [Column("USERNAME")]
        public string? Username { get; set; }

        [Column("PASSWORD")]
        public byte[]? Password { get; set; }

        [Column("LASTNAME")]
        public string? LastName { get; set; }

        [Column("FIRSTNME")]
        public string? FirstName { get; set; }

        // ADDITION FIELDS
        [Column("ZIPCODE")]
        public string? Zipcode { get; set; } = string.Empty;

        [Column("AREA")]
        public string? Area { get; set; } = string.Empty;

        [Column("DEPUTYNAMES")]
        public string? DeputyNames { get; set; } = string.Empty;

        [Column("DEPUTYPHONE")]
        public string? DeputyPhone { get; set; } = string.Empty;

        [Column("DEPUTYEMAIL")]
        public string? DeputyEmail { get; set; } = string.Empty;

        [Column("DEPUTYYESNO")]
        public decimal? DeputyYesNo { get; set; } = decimal.Zero;

        [Column("NAMES")]
        public string? Names { get; set;}
    }
}