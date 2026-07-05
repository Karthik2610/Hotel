using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelProject.Models
{
	public class BookingDetails
	{
		[Key]
		public int BookingID { get; set; }
		
		public int CutomerID { get; set; }
		[ForeignKey("CutomerID")]
		public virtual CustomerDetails? CustomerDetails { get; set; } = null;
		//public int RoomID { get; set; }
		//[ForeignKey("RoomID")]
		//public virtual RoomsLU? RoomsLU { get; set; } = null;
		public decimal? AdvanceAmount { get; set; }
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
		//public decimal BalanceAmount { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; } = string.Empty;
		[StringLength(255)]
		public string? BookingStatus { get; set; }
		public DateTime? CheckInTime { get; set; }
		public DateTime? CheckOutTime { get; set; }
	}
}
