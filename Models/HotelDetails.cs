using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace HotelProject.Models
{
	public class HotelDetails
	{
		[Key]
		public int HotelID { get; set; }
		public string? Name { get; set; } = string.Empty;
		public string? Address1 { get; set; } = string.Empty; 
		public string? Address2 { get; set; } = string.Empty;

		public string? City { get; set; } = string.Empty;
		public string? State { get; set; } = string.Empty;
		public string? PinCode { get; set; } = string.Empty;
		public string? GSTNumber { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; } = string.Empty;
		public string? MobileNumber1 { get; set; } = string.Empty;
		public string? MobileNumber2 { get; set; } = string.Empty;
		[EmailAddress]
		public string? Email { get; set; } = string.Empty;
		public DateTime? CreatedDate { get; set; }
		public string? CreatedBy { get; set; } = string.Empty;
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; } = string.Empty;
		public string? Website { get; set; } = string.Empty;
		public decimal? CGST { get; set; } 
		public decimal? SGST { get; set; } 
		public decimal? ExtraBedCost { get; set; }
	}
}
