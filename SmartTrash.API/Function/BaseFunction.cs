using MySqlConnector;
using System.Data;

namespace SmartTrash.API.Function
{
    public class BaseFunction
    {
        protected string connectionString;
        protected MySqlConnection mySqlConnection;

        public BaseFunction(IConfiguration configuration)
        {
            // Khai báo thông tin kết nối
            connectionString = configuration.GetConnectionString("dataBase");
            // Khai báo tên bảng
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
    }
}
