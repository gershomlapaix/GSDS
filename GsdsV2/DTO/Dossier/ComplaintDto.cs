namespace GsdsV2.DTO.Dossier
{
    public class ComplaintDto
    {
        public string ComplaintCode { get; set; }
        public DateTime TransferDate { get; set; }
        public string? ComplainerId { get; set; }
        public string? AccusedIdNumber { get; set; }
        public string? Subject { get; set; }
        public string? AccusedComment { get; set; }
        //public string? Attachments { get; set; }
        public string? previousInstitutions { get; set; }
        public string? ComplaintOwner { get; set; }
        public string? ProvinceId { get; set; }
        public string? DistrictId { get; set; }
        public string? SectorId { get; set; }
        public string? CellId { get; set; }
        public string? StartOffice { get; set; }
        public string? ComplaintCategoryId { get; set; } 
        public string? RoleId { get; set; }

    }
}
