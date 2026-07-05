using System.Data;

namespace HotelProject.Models.ViewModel
{
	public class CheckOutViewModel
	{

		public HotelDetails HotelDetails { get; set; }
		public CustomerDetails CustomerDetails { get; set; }
		public DataTable RoomDetails { get; set; }
		public int? BookingID { get; set; }
		public string? AmountInWords { get; set; }
	}
}
