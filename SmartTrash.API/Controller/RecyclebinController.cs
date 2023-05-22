using Dapper;
using Microsoft.AspNetCore.Mvc;
using SmartTrash.API.Entity;

namespace SmartTrash.API.Controller
{
    public class RecycleBinController : BaseController<RecycleBin>
    {
        public RecycleBinController(IConfiguration configuration) : base(configuration)
        {
            
        }

        /// <summary>
        /// Clear các thùng rác đã được thu gom
        /// </summary>
        /// <param name="ids">Chuỗi id các thùng rác, ngăn cách bởi dấu ; </param>
        /// <returns></returns>
        [HttpPost("clearTrash")]
        public IActionResult ClearTrash([FromBody] string ids)
        {
            // Tách dữ liệu id từ chuỗi ids:
            List<string> selectedIds = ids.Split(';').ToList();
            // Chuẩn bị câu lệnh 
            var getCommand = $"Proc_Recyclebin_ClearTrash";
            // Chuẩn bị tham số đầu vào cho câu lệnh sql
            var parameters = new DynamicParameters();

            var whereClause = "";
            foreach (var id in selectedIds)
            {
                if (id.Equals(selectedIds.Last()))
                {
                    whereClause += $"'{id}'";
                }
                else
                {
                    whereClause += $"'{id}', ";
                }
            }
            whereClause = '"' + whereClause + '"';

            parameters.Add("ids", whereClause);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Execute(getCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
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
