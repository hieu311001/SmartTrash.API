using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("recyclebinplan")]
    public class RecycleBinPlan
    {
        [Key]
        public Guid RecycleBinPlanID { get; set; }

        [Required]
        public Guid RecycleBinID  { get; set; }

        public string? RecycleBinName { get; set; }

        [Required]
        public string RecycleBinPlanName { get; set; }

        [Required]
        public RecyclebinplanType RecycleBinPlanType { get; set; }
    }
}
