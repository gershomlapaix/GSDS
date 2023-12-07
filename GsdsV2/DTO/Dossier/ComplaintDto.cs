using Gsds.Models.Dossier;
using GsdsV2.Models.Dossier;
using GsdsV2.Models.HelperModels;

namespace GsdsV2.DTO.Dossier
{
    public class ComplaintDto
    {
        public string ComplaintCode { get; set; }
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
        //public string? ComplaintCategoryId { get; set; }

        public bool? IsCourtJudgementReview { get; set; }

        public ComplaintDto() { }
        public ComplaintDto(Complaint complaint) =>
            (ComplaintCode,ComplainerId, AccusedIdNumber, Subject, AccusedComment, previousInstitutions, ComplaintOwner,
            ProvinceId, DistrictId, SectorId, CellId, IsCourtJudgementReview) = 
            (complaint.ComplaintCode, complaint.ComplainerId, complaint.AccusedIdNumber, complaint.Subject, complaint.AccusedComment,
            complaint.previousInstitutions, complaint.ComplaintOwner, complaint.ProvinceId, complaint.DistrictId, complaint.SectorId, complaint.CellId,
            complaint.IsCourtJudgementReview);
    }
}
