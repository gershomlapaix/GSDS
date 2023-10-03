using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Dossier
{
    [Table("COMPLAINT_MANAGEMENT", Schema ="ADMIN")]
    public class ComplaintManagement
    {
        [Key]
        [Column("SEQ_NUMBER")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Double SeqNumber { get; set; }

        [Column("TRANS_DATE")]
        public DateTime TransDate { get; set; }

        [Column("COMPLAINT_CODE")]
        public string? ComplaintCode { get; set; }

        [Column("USERNAME")]
        public string? Username { get; set; }

        [Column("CODE_MGR")]
        public string? CodeMngr { get; set; } = "00001";

        [Column("LEVEL_FROM")]
        public string? LevelFrom { get; set; }

        [Column("LEVEL_TO")]
        public string? LevelTo { get; set;}

        [Column("RESPONSIBLE")]
        public string? Responsible { get; set; } = string.Empty;

        [Column("REDIRECTED")]
        public decimal? Redirected { get; set; } = 0;

        [Column("INSTITUTION")]
        public string? InstitutionId { get; set; }

        [Column("DUE_DATE")]
        public DateTime DueDate { get; set; }

        [Column("RESPONSIBLEFROM")]
        public string? ResponsibleFrom { get; set; } = string.Empty;

        [Column("INTERNAL_COMMENT")]
        public string? InternalComment { get; set; } = null;

        [Column("EXTERNAL_COMMENT")]
        public string? ExternalComment { get; set; } = null;

        [Column("STATUSFIRST")]
        public decimal? StatusFirst { get; set; } = 0;

        [Column ("CC")]
        public string? Cc { get; set; }
    }
}
