// This class contains the details about the complainer
using System.ComponentModel.DataAnnotations.Schema;
using Gsds.Constants.Dossier;
using GsdsAuth.Models;
using GsdsV2.Models.Dossier;
using GsdsV2.Models.HelperModels;

namespace Gsds.Models.Dossier
{
    [Table("PLAINTIFF", Schema = "ADMIN")]
    public class Complainer
    {

        [Column("ID_NUMBER")]
        public string? Id { get; set; }

        [Column("ID_PERSON_TYPE")]
        public string? PeronTypeId { get; set; }

        [Column("SEX")]
        public string? GenderId { get; set; }

        [Column("ID_TYPE")]
        public string? IdType { get; set; }

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

        [Column("ASSIGNED_TO")]
        public string? RoleIdAssignedTo { get; set;}

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
        public decimal? IsDeputy { get; set; } = decimal.Zero;

        [Column("NAMES")]
        public string? TheNames { get; set; }

        // navigation properties
        public List<Complaint> Complaints { get; set; }
        public Province? Province { get; set; }
        public District? District { get; set; }
        public Sector? Sector { get; set; }
        public Cell? Cell { get; set; }
        public Gender? Gender { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public IdentifierType? IdentifierType { get; set; }
        public PersonType? PersonType { get; set; }
        public List<ManagerRoles>? ManagerRoles { get; set; }
        public List<ComplainerAssignedToRole>? AssignedToRoles { get; set; }

    }
}