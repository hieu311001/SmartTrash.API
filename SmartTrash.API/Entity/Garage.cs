using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("garage")]
    public class Garage
    {
        [Key]
        public Guid GarageID { get; set; }

        [Required]
        public string? Location { get; set; }

    }
}
