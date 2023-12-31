﻿using Gsds.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("PERSON_TYPE", Schema = "ADMIN")]
    public class PersonType
    {
        [Column("ID_PERSON_TYPE")]
        public string Id { get; set; }

        [Column("PERSON_TYPE")]
        public string PersonTypeName { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        public List<Complainer>? Complainers { get; set; }
        public List<Accused> Accuseds { get; set; }

    }
}
