using System.ComponentModel.DataAnnotations;

namespace HotelProject.Models
{
	public class CustomerDetails
	{
		[Key]
		public int CustomerID { get; set; }
		[Display(Name = "First Name")]
		[Required]
		public string? FirstName { get; set; } = string.Empty; 
		[Display(Name = "Last Name")]
		public string? LastName { get; set; } = string.Empty;
		[Display(Name = "Email Address")]
		[EmailAddress]
		public string? Email { get; set; } = string.Empty;
		[Display(Name = "Phone Number")]
		[Required]
		public string PhoneNumber { get; set; } = string.Empty;
		[Display(Name = "Address Line 1")]
		public string? Address1 { get; set; } = string.Empty;
		[Display(Name = "Address Line 2")]
		public string? Address2 { get; set; } = string.Empty;
		[Display(Name = "GST Number")]
		public string? GST { get; set; } = string.Empty;
		[Display(Name = "City")]
		public string? City { get; set; } = string.Empty;
		[Display(Name = "State")]
		public string? State { get; set; } = string.Empty;
		[Display(Name = "Pin Code")]
		public string? PinCode { get; set; } = string.Empty;
		public DateTime? CreatedDate { get; set; }
		public string? CreatedBy { get; set; } = string.Empty;
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; } = string.Empty;

		[Display(Name = "Aadhar Card Number")]
		public string? AadharCardNumber { get; set; } = string.Empty;
		[Display(Name = "Pan Card Number")]
		public string? PanCardNumber { get; set; } = string.Empty;
		[Display(Name = "Driving License Number")]
		public string? DrivingLicenseNumber { get; set; } = string.Empty;
		[Display(Name = "Passport Number")]
		public string? PassportNumber { get; set; } = string.Empty;
		[Display(Name = "Other Identification")]
		public string? Others { get; set; } = string.Empty;

	}
}
