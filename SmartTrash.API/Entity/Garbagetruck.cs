using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("garbagetruck")]
    public class GarbageTruck
    {
        [Key]
        public Guid GarbageTruckID { get; set; }

        [Required]
        public int Usages { get; set; }
        [Required]
        public GarbageTruckStatus Status { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public Guid GarageID { get; set; }
        public Guid? TemporaryGarageID { get; set; }
        public string? RecycleBinIDList { get; set; }
    }
}
