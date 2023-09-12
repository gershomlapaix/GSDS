using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Dossier
{
    [Table("COMPLAINT", Schema ="ADMIN")]
    public class Complaint
    {
        [Key]
        [Column("COMPLAINT_CODE")]
        public string ComplaintCode { get; set; }

        [Column("TRANS_DATE")]
        public DateTime TransferDate { get; set;}

        [Column("CMPLNR_ID_NUMBER")]
        public string? ComplainerId { get; set; }

        [Column("ACCUSED_ID_NUMBER")]
        public string? AccusedIdNumber { get; set; }

        [Column("SUBJECT")]
        public string? Subject { get; set; }

        [Column("COMPLAINT_DETAILOLD")]
        public string? ComplaintDetail { get; set; }

        [Column("RAA_PAY_REF")]
        public string? RaaPayRef { get; set; }

        [Column("COMPLAINT_PLACE")]
        public string? ComplaintPlace { get; set; }

        [Column("ACCUSED_COMMENT")]
        public string? AccusedComment { get; set; }

        [Column("DUE_DATE")]
        public DateTime? DueDate { get; set; }

        [Column("ATTACHMENT")]
        public string? Attachments { get; set;}

        [Column("ID_CATEGORY")]
        public string? CategoryId { get; set; }

        [Column("ID_PRIORITY")]
        public string? PriorityId { get; set; }

        [Column("STATUS_CODE")]
        public string? StatusCode { get; set; }

        [Column("ID_ROLE")]
        public string? RoleId { get; set; }

        [Column("COMPLAINT_DETAIL")]
        public string? complaintDescription { get; set; }

        [Column("UREGERA")]
        public string? Uregera { get; set; }

        [Column("PREVIOUSINSITUTIONS")]
        public string? previousInstitutions { get; set; }

        [Column("COMPLAINTOWNER")]
        public string? ComplaintOwner { get; set; }

        [Column("STATUSFIRST")]
        public decimal? StatusFirst { get; set; }

        // LOCATION
        [Column("PROVINCE_ID")]
        public string? ProvinceId { get; set; }

        [Column("DISTRICT_ID")]
        public string? DistrictId { get; set; }

        [Column("SECTOR_ID")]
        public string? SectorId { get; set; }

        [Column("CELL_ID")]
        public string? CellId { get; set; }

        [Column("StatLocation")]
        public string? StartOffice { get; set; }


        // NAVIGATION PROPERTIES
    }
}
