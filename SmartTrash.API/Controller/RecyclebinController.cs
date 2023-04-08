using SmartTrash.API.Entity;

namespace SmartTrash.API.Controller
{
    public class RecycleBinController : BaseController<RecycleBin>
    {
        public RecycleBinController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
