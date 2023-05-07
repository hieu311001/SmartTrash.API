using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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