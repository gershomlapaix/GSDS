// This class contains the details about the complainer
using System.ComponentModel.DataAnnotations.Schema;
using Gsds.Constants.Dossier;
using GsdsAuth.Models;

namespace Gsds.Models.Dossier{
    [Table("PLAINTIFF", Schema = "ADMIN")]
    public class Complainer {
        public string Id { get; set; }

        [Column("ID_PERSON_TYPE")]
        public char personTypeId { get; set; }

        [Column("SEX")]
        public char GenderId { get; set; }

        [Column("ID_NUMBER")]
        public string IdNumber { get; set; }

        [Column("ID_DETAILS")]
        public string IdDetails { get; set; }

        [Column("DOB")]
        public DateTime birthDate { get; set; }

        [Column("DATE_CREATION")]
        public DateTime RegistrationDate { get; set; }

        [Column("ID_MARITAL_STATUS")]
        public char MaritalStatusId { get; set; }

        [Column("TELEPHONE")]
        public string Phone { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("PO_BOX")]
        public string PoBox { get; set; }

        // LOCATION
        [Column("PROVINCE_ID")]
        public char ProvinceId { get; set; }

        [Column("DISTRICT")]
        public char DistrictId { get; set; }

        [Column("SECTOR_ID")]
        public char SectorId { get; set; }

        [Column("CELL_ID")]
        public char CellId { get; set; }

        [Column("ADDRESS")]
        public string Address { get; set; }

        // RELATED TO THE USER OBJECT

        [Column("USERNAME")]
        public string Username { get; set; }

        //[Column("PASSWORD")]
        //public byte[] Password { get; set; }

        [Column("LASTNAME")]
        public string LastName { get; set; }

        [Column("FIRSTNAME")]
        public string FirstName { get; set; }

        

    }
}