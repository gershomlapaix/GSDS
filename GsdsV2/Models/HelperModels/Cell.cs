using System.ComponentModel.DataAnnotations.Schema;

namespace GsdsV2.Models.HelperModels
{
    [Table("CELL", Schema ="ADMIN")]
    public class Cell
    {

        [Column("CELL_ID")]
        public string Id { get; set; }

        [Column("SECTOR_ID")]
        public string SectorId { get; set; }

        [Column("CELL_NAME")]
        public string CellName { get; set;}
    }
}
