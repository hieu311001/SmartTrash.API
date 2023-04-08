using SmartTrash.API.Entity;

namespace SmartTrash.API.Controller
{
    public class GarageController : BaseController<Garage>
    {
        public GarageController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
