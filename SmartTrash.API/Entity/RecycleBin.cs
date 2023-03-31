using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("recyclebin")]
    public class RecycleBin
    {
        [Key]
        public Guid RecycleBinID { get; set; }

        [Required]
        public Guid LocationID { get; set; }

        public string? LocationName { get; set; }

        [Required]
        public string RecycleBinName { get; set; }

        [Required]
        public RecycleBinStatus RecyclebinStatus { get; set; }
    }
}
