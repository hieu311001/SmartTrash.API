using SmartTrash.API.Entity;

namespace SmartTrash.API.Controller
{
    public class NotificationController : BaseController<Notification>
    {
        public NotificationController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
