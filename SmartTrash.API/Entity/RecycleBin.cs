using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("recyclebin")]
    public class RecycleBin
    {
        public Guid RecycleBinID { get; set; }

        public RecycleBinStatus RecyclebinStatus { get; set; }

        public int Usages { get; set; }

        public int DaySinceLastCollection { get; set; }

        public string? Location { get; set; }

        public string RecycleBinName { get; set; }
    }
}
