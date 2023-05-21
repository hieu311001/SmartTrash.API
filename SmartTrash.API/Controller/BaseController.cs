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
        protected string apiKey;

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
            var getCommand = $"Proc_{className}_GetAll";

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Query<T>(getCommand, commandType: System.Data.CommandType.StoredProcedure);
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

        [HttpGet("id")]
        public IActionResult GetByID([FromQuery] Guid id)
        {
            // Chuẩn bị câu lệnh 
            var getCommand = $"Proc_{className}_GetByID";

            var parameters = new DynamicParameters();
            parameters.Add($"${className}ID", id);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.QueryFirstOrDefault<T>(getCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
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

        [HttpPost]
        public IActionResult Insert(T entity)
        {
            // Chuẩn bị câu lệnh 
            var insertCommand = $"Proc_{className}_Insert";

            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters(entity);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Execute(insertCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về ở db
            if (result != 0)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(T entity, Guid id)
        {
            // Chuẩn bị câu lệnh 
            var updateCommand = $"Proc_{className}_Update";

            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters(entity);
            parameters.Add($"{className}ID", id);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Execute(updateCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về ở db
            if (result != 0)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            // Chuẩn bị câu lệnh 
            var deleteCommand = $"Proc_{className}_Delete";

            // Chuẩn bị các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"${className}ID", id);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Execute(deleteCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về ở db
            if (result != 0)
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
