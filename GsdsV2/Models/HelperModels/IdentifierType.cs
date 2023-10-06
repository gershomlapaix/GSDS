using Gsds.Models.Dossier;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("IDENTIFIER_TYPE", Schema = "ADMIN")]
    public class IdentifierType
    {
        [Column("ID_TYPE")]
        public string Id { get; set; }

        [Column("TYPE_NAME")]
        public string TypeName { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("ID_PERSON_TYPE")]
        public string? IdPersonType { get; set; }

        public List<Complainer> Complainers { get; set; }
        public List<Accused> Accuseds { get; set; }

    }
}
