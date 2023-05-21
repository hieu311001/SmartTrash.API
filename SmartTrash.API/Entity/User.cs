
using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("user")]
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Email { get; set; }

        public UserType? UserType { get; set; }
    }
}
