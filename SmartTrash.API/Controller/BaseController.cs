using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace SmartTrash.API.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseController<T> : ControllerBase
    {
        protected string connectionString;
        protected MySqlConnection mySqlConnection;
        protected string className;

        public BaseController(IConfiguration configuration)
        {
            // Khai báo thông tin kết nối
            connectionString = configuration.GetConnectionString("dataBase");
            // Khai báo tên bảng
            className = typeof(T).Name;
            mySqlConnection = new MySqlConnection(connectionString);
            if (mySqlConnection != null && mySqlConnection.State != ConnectionState.Open)
            {
                mySqlConnection.Open();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Chuẩn bị câu lệnh 
            var getAllEmulationCommand = $"Proc_{className}_GetAll";

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Query<T>(getAllEmulationCommand, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về ở db
            if (result != null)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {

                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
