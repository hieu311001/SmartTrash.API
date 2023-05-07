using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
	[Table("routenode")]
	public class RouteNode
	{
		// ID của điểm bắt đầu
		public Guid OriginID { get; set; }

		// ID điểm đến
		public Guid DestinationID { get; set; }

		// Khoảng cách
		public double Distance { get; set; }

		// Mức độ ưu tiên
		public double Prioritize { get; set; }

		// Kiểm tra điểm bắt đầu là của xe hay của thùng rác
		public RouteNodeType RouteNodeType { get; set; }

		// Thứ tự theo xe
		public int SortOrder { get; set; }
	}
}