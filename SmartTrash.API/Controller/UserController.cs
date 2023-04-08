using SmartTrash.API.Entity;

namespace SmartTrash.API.Controller
{
    public class UserController : BaseController<User>
    {
        public UserController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
