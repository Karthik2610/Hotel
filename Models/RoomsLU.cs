using System.ComponentModel.DataAnnotations;

namespace HotelProject.Models
{
	public class RoomsLU
	{
		[Key]
		public int RoomID { get; set; }
		public string RoomName { get; set; } = string.Empty;
		public string RoomType { get; set; } = string.Empty;
		public int TotalRooms { get; set; }

		public decimal Price { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; } = string.Empty;

	}
}
