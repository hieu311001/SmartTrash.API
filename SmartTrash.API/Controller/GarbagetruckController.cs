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

        /// <summary>
        /// Tìm đường đi
        /// TDHIEU
        /// </summary>
        /// <returns></returns>
        [HttpGet("str")]
        public IActionResult FindWay()
        {
            // Chuẩn bị câu lệnh 
            var garbageTruckCommand = $"Proc_GarbageTruck_GetAll";
            var recycleBinCommand = $"Proc_RecycleBin_GetAll";

            // Thực hiện gọi vào db để chạy câu lệnh 
            var garbageTruckResult = mySqlConnection.Query<GarbageTruck>(garbageTruckCommand, commandType: System.Data.CommandType.StoredProcedure).ToList();
            var recycleBinResult = mySqlConnection.Query<RecycleBin>(recycleBinCommand, commandType: System.Data.CommandType.StoredProcedure).ToList();

            // Lấy dữ liệu khoảng cách
            List<RouteNode> routeResult = new List<RouteNode>();

            // 1. Lấy dữ liệu khoảng cách giữa xe thu gom với các thùng rác
            foreach (var truckItem in garbageTruckResult)
            {
                foreach (var binItem in recycleBinResult)
                {
                    string url = $"https://maps.googleapis.com/maps/api/directions/json?key={apiKey}&origin={truckItem.Location.Replace(" ", "")}&destination={binItem.Location.Replace(" ", "")}";
                    string requesturl = url;

                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(requesturl);

                    string sContents = string.Empty;
                    sContents = System.Text.Encoding.ASCII.GetString(response);
                    JObject o = JObject.Parse(sContents);
                    try
                    {

                        RouteNode node = new RouteNode();
                        node.OriginID = truckItem.GarbageTruckID;
                        node.DestinationID = binItem.RecycleBinID;
                        node.Distance = (double)o.SelectToken("routes[0].legs[0].distance.value");
                        node.Prioritize = node.Distance * binItem.DaySinceLastCollection * binItem.Usages / 100;
                        node.RouteNodeType = Enum.RouteNodeType.GarbageTruck;
                        routeResult.Add(node);
                    }
                    catch
                    {
                    }
                }
            }

            // 2. Lấy dữ liệu khoảng cách giữa các thùng rác với nhau
            for (int i = 0; i < recycleBinResult.Count() - 1; i++)
            {
                for (int j = i + 1; j < recycleBinResult.Count(); j++)
                {
                    string url = $"https://maps.googleapis.com/maps/api/directions/json?key={apiKey}&origin={recycleBinResult[i].Location.Replace(" ", "")}&destination={recycleBinResult[j].Location.Replace(" ", "")}";
                    string requesturl = url;

                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(requesturl);

                    string sContents = string.Empty;
                    sContents = System.Text.Encoding.ASCII.GetString(response);
                    JObject o = JObject.Parse(sContents);
                    try
                    {

                        RouteNode node = new RouteNode();
                        node.OriginID = recycleBinResult[i].RecycleBinID;
                        node.DestinationID = recycleBinResult[j].RecycleBinID;
                        node.Distance = (double)o.SelectToken("routes[0].legs[0].distance.value");
                        node.Prioritize = node.Distance * recycleBinResult[j].DaySinceLastCollection * recycleBinResult[j].Usages / 100;
                        node.RouteNodeType = Enum.RouteNodeType.RecycleBin;
                        routeResult.Add(node);
                    }
                    catch
                    {
                    }
                }
            }

            // 3. Tính toán đường đi ngắn nhất cho mỗi xe
            // Mỗi xe nhận 5 thùng rác
            List<RouteNode> routeFinding = new List<RouteNode>();

            for (int i = 0; i < garbageTruckResult.Count(); i++)
            {
                // Tìm thùng rác đầu tiên có độ ưu tiên cao nhất
                var tempTruckNodes = routeResult.Where(item => item.OriginID == garbageTruckResult[i].GarbageTruckID).OrderBy(item => item.Prioritize);
                if (tempTruckNodes.Count() > 0)
                {
                    var truckNode = tempTruckNodes.Last();
                    truckNode.SortOrder = i + 1;
                    routeFinding.Add(truckNode);

                    // Xóa đi tất cả các node của các xe còn lại có chung điểm đến hoặc của xe đang xét
                    routeResult.RemoveAll(item => item.RouteNodeType == Enum.RouteNodeType.GarbageTruck && (item.DestinationID == truckNode.DestinationID || item.OriginID == truckNode.OriginID));
                }

                for (int j = 1; j <= 4; j++)
                {
                    // Tìm các thùng rác tiếp theo
                    if (routeFinding.Count() > 0)
                    {
                        var lastNode = routeFinding.Last();
                        var tempBinNodes = routeResult.Where(item => item.RouteNodeType == Enum.RouteNodeType.RecycleBin && item.OriginID == lastNode.DestinationID).OrderBy(item => item.Prioritize);

                        if (tempBinNodes.Count() > 0)
                        {
                            var binNode = tempBinNodes.Last();
                            binNode.SortOrder = i + 1;
                            routeFinding.Add(binNode);

                            // Xóa đi tất cả các node có điểm bắt đầu hoặc điểm đến giống với node đã chọn
                            routeResult.RemoveAll(item => item.OriginID == binNode.OriginID || item.DestinationID == binNode.OriginID || item.DestinationID == binNode.DestinationID);
                        }
                    }
                }
            }

            // 4. Gán lại kết quả tìm đường vào db
            var resetRouteCommand = $"Proc_GarbageTruck_ResetRoute";

            var result = 0;

            // Chuẩn bị các tham số đầu vào
            for (int i = 0; i < garbageTruckResult.Count(); i++)
            {
                var tempNodes = routeFinding.Where(item => item.SortOrder == i + 1);
                if (tempNodes.Count() > 0)
                {
                    Guid garbageTruckID = tempNodes.First().OriginID;
                    string recycleBinIDs = tempNodes.First().DestinationID + ";";
                    foreach (var node in tempNodes.Where(item => item.RouteNodeType == Enum.RouteNodeType.RecycleBin))
                    {
                        recycleBinIDs += node.DestinationID + ";";
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add($"garbageTruckID", garbageTruckID);
                    parameters.Add($"recycleBinIDs", recycleBinIDs);

                    // Thực hiện gọi vào db để chạy câu lệnh 
                    result = mySqlConnection.Execute(resetRouteCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
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