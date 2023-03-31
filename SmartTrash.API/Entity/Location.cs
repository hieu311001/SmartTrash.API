using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("location")]
    public class Location
    {
        [Key]
        public Guid LocationID { get; set; }
        [Required]
        public string LocationName { get; set; }
    }
}
