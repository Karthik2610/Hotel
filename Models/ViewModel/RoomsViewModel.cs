namespace HotelProject.Models.ViewModel
{
	public class RoomsViewModel
	{
		public int RoomID { get; set; }
		public string? RoomName { get; set; } = string.Empty;
		public bool IsSelected { get; set; }
		public int TotalRooms { get; set; } = 0;

		public int ExtraBeds { get; set; }
	}
}
