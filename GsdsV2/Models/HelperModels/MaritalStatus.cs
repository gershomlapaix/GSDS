﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("MARITAL_STATUS", Schema ="ADMIN")]
    public class MaritalStatus
    {
        [Column("ID_MARITAL_STATUS")]
        public string Id { get; set; }

        [Column("MARITAL_STATUS")]
        public string MaritalStatusName { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
