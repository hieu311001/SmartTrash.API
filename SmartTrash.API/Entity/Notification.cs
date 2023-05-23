using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("notification")]
    public class Notification
    {
        public Guid NotificationID { get; set; }

        public Guid RecycleBinID { get; set; }

        public NotificationType NotificationType { get; set; }
        public string? NotificationName { get; set; }    

        public string? RecycleBinName { get; set; }
    }
}
