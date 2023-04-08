using SmartTrash.API.Entity;

namespace SmartTrash.API.Controller
{
    public class GarbagetruckController : BaseController<GarbageTruck>
    {
        public GarbagetruckController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
