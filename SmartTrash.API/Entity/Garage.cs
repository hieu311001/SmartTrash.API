using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("garage")]
    public class Garage
    {
        public Guid GarageID { get; set; }
        public string? Location { get; set; }

        public string GarageName { get; set; }

    }
}
