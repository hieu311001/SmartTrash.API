using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("notification")]
    public class Notification
    {
        [Key]
        public Guid NotificationID { get; set; }

        [Required]
        public Guid RecycleBinID { get; set; }

        public string? RecycleBinName { get; set; }
        [Required]
        public NotificationType NotificationType { get; set; }
        [Required]
        public string NotificationName { get; set; }    

    }
}
