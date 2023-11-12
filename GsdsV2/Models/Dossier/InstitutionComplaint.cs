﻿using GsdsV2.Models.HelperModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.Dossier
{
    [Table("INSTITUTION_COMPLAINTS", Schema ="ADMIN")]
    public class InstitutionComplaint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("INSTITUTION_ID")]
        public double? InstitutionId { get; set; }

        [Column("COMPLAINT_CODE")]
        public string? complaintCode { get; set; }

        // Navigation properties
        public Complaint? Complaint { get; set; }
        public Institution? Institution { get; set; }
    }
}