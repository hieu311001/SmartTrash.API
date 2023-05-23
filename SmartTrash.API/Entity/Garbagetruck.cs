using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("garbagetruck")]
    public class GarbageTruck
    {
        public Guid GarbageTruckID { get; set; }

        public int Usages { get; set; }
        public GarbageTruckStatus Status { get; set; }
        public string? Location { get; set; }
        public Guid GarageID { get; set; }
        public Guid? TemporaryGarageID { get; set; }
        public string? RecycleBinIDList { get; set; }

        public string GarbageTruckName { get; set; }
    }
}
