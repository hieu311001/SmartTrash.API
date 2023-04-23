using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using SmartTrash.API.Entity;
using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartTrash.API.Function
{
    public class UserFunction
    {

        protected string connectionString;
        protected MySqlConnection mySqlConnection;

        public IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public UserFunction(IMemoryCache cache)
        {
            _cache = cache;
        }

        public UserFunction()
        {
            // Khai báo thông tin kết nối
            connectionString = "Server=localhost;Port=3306;Database=smart_trash;Uid=root;Pwd=Hieu311001.";
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

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("yourSecretKeyApplication");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = "yourIssuer",
                Audience = "yourAudience",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _cache.Set($"{user.UserName}", tokenString, DateTimeOffset.Now.AddMinutes(30));

            return tokenString;
        }

        public User CheckSignIn(string UserName, string Password)
        {
            // Chuẩn bị câu lệnh 
            var getCommand = $"Proc_User_SignIn";
            var parameters = new DynamicParameters();
            parameters.Add($"UserName", UserName);
            parameters.Add($"PassWord", Password);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.QueryFirstOrDefault<User>(getCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return result;
        }

        public User GetByName(string UserName)
        {
            // Chuẩn bị câu lệnh 
            var getCommand = $"Proc_User_GetByName";

            var parameters = new DynamicParameters();
            parameters.Add($"$UserName", UserName);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.QueryFirstOrDefault<User>(getCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về ở db
            return result;
        }


        public int SignUp(User user)
        {
            // Chuẩn bị câu lệnh 
            var getCommand = $"Proc_User_Insert";

            var parameters = new DynamicParameters(user);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Execute(getCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về ở db
            return result;
        }

        public void SignOut(string userName)
        {
            _cache.Remove(userName);
        }

        public T GetCache<T>(string key)
        {
            return _cache.Get<T>($"{key}");
        }
    }
}
