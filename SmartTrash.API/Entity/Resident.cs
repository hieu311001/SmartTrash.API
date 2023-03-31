using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("resident")]
    public class Resident
    {
        [Key]
        public Guid ResidentID { get; set; }

        [Required]
        public Guid LocationID { get; set; }

        public string? LocationName { get; set; }

        [Required] 
        public int ResidentNumber { get; set; }

        [Required]
        public int RecycleBinFullTime { get; set; }

        [Required]
        public string ResidentName { get; set; }
    }
}
