using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace HotelProject.Models.ViewModel
{
	public class SearchViewModel
	{
		[Display(Name = "Check In Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		[Required(ErrorMessage = "Check In Date is required")]
		public DateTime? CheckInDate { get; set; }
		[Display(Name = "Check Out Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		[Required(ErrorMessage = "Check Out Date is required")]
		public DateTime? CheckOutDate { get; set; }

		public DataTable? ListModel { get; set; }
		public CustomerDetails? customerDetails { get; set; }
		public string? SearchAction { get; set; }
		public string? CreateEditAction { get; set; }
		[Display(Name = "Advance Amount")]
		public decimal? AdvanceAmount { get; set; }
		public List<RoomsViewModel>? RoomsViewModel { get; set; }
		public int? BookingID { get; set; }
	}


}
