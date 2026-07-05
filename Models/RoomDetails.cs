using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelProject.Models
{
	public class RoomDetails
	{
		[Key]
		public int RoomDetailsId { get; set; }
		public int BookingID { get; set; }
		[ForeignKey("BookingID")]
		public virtual BookingDetails? BookingDetails { get; set; } = null;
		public int RoomID { get; set; }
		[ForeignKey("RoomID")]
		public virtual RoomsLU? RoomsLU { get; set; } = null;
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; } = string.Empty;

		public int? ExtraBeds { get; set; }
	}
}
