using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GsdsV2.DTO.Dossier
{
    public class ComplaintManagementDto
    {
        public DateTime TransDate { get; set; }

        //public string? ComplaintCode { get; set; }

        public string? Username { get; set; }

        //public string? LevelFrom { get; set; }

        public string? LevelTo { get; set; }

        public string? InstitutionId { get; set; }

        public int DueDate { get; set; }
        //public string? Responsible { get; set; }
        public string? ResponsibleFrom { get; set; }

        public string? InternalComment { get; set; }

        public string? ExternalComment { get; set; }

        public string? CcRole { get; set; }
    }
}
