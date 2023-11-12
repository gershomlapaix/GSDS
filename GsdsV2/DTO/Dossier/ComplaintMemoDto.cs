using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.DTO.Dossier
{
    public class ComplaintMemoDto
    {
        public string? UsernameTo { get; set; }

        public string? Title { get; set; }

        public string? Details { get; set; }
    }
}
