using HotelProject.Data;
using HotelProject.Models;
using HotelProject.Models.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace HotelProject.Services
{
	public class BookingService
	{

		private readonly ApplicationDbContext _context;
		private readonly ExecuteSP _executeSP;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public BookingService(ApplicationDbContext context, ExecuteSP executeSP, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_executeSP = executeSP;
			_httpContextAccessor = httpContextAccessor;
		}
		public DataTable RoomDetails(DateTime checkInDate, DateTime checkOutDate)
		{
			DataTable dt = _executeSP.ExecuteSPAsync("spBookingDetails", new SqlParameter("@CheckInDate", checkInDate), new SqlParameter("@CheckOutDate", checkOutDate)).Result;
			return dt; // Room is available if there are no conflicting bookings
		}
		public DataTable GetRoomsForBooking(int bookingID, DateTime checkInDate, DateTime checkOutDate)
		{
			DataTable dt = _executeSP.ExecuteSPAsync("spGetRoomsForBooking", new SqlParameter("@BookingID", bookingID), new SqlParameter("@CheckInDate", checkInDate), new SqlParameter("@CheckOutDate", checkOutDate)).Result;
			return dt; // Room is available if there are no conflicting bookings
		}
		public DataTable CheckOutDetails(int bookingID)
		{
			DataTable dt = _executeSP.ExecuteSPAsync("spCheckOutBill", new SqlParameter("@BookingID", bookingID)).Result;
			return dt; // Room is available if there are no conflicting bookings
		}
		public BookingDetails GetBookingDetails(int bookingID)
		{
			return _context.BookingDetails.Find(bookingID);
		}
		public bool CancelBooking(SearchViewModel model)
		{
			using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
			{
				try
				{
					_context.RoomDetails.RemoveRange(_context.RoomDetails.Where(rd => rd.BookingID == model.BookingID.Value));
					_context.SaveChanges();
					BookingDetails bookingDetails = _context.BookingDetails.Find(model.BookingID.Value);
					bookingDetails.BookingStatus = "Cancelled";
					bookingDetails.ModifiedDate = DateTime.Now;
					bookingDetails.ModifiedBy = GetCurrentUserId();
					_context.BookingDetails.Update(bookingDetails);
					_context.SaveChanges();
					transaction.Commit(); // Commit the transaction if all operations succeed
					return true;
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					return false; // Indicate failure
				}
			}
		}
		public bool SaveCustomerDetails(SearchViewModel model)
		{
			using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
			{
				try
				{
					CustomerDetails customerDetails = _context.CustomerDetails.Find(model.customerDetails.CustomerID);
					customerDetails.PassportNumber = model.customerDetails.PassportNumber;
					customerDetails.DrivingLicenseNumber = model.customerDetails.DrivingLicenseNumber;
					customerDetails.PanCardNumber = model.customerDetails.PanCardNumber;
					customerDetails.AadharCardNumber = model.customerDetails.AadharCardNumber;
					customerDetails.Others = model.customerDetails.Others;
					customerDetails.ModifiedDate = DateTime.Now;
					customerDetails.ModifiedBy = GetCurrentUserId();
					customerDetails.Address1 = model.customerDetails.Address1;
					customerDetails.Address2 = model.customerDetails.Address2;
					customerDetails.City = model.customerDetails.City;
					customerDetails.State = model.customerDetails.State;
					customerDetails.PinCode = model.customerDetails.PinCode;
					customerDetails.Email = model.customerDetails.Email;
					customerDetails.PhoneNumber = model.customerDetails.PhoneNumber;
					customerDetails.GST = model.customerDetails.GST;
					customerDetails.FirstName = model.customerDetails.FirstName;
					customerDetails.LastName = model.customerDetails.LastName;
					
					_context.CustomerDetails.Update(customerDetails);
					_context.SaveChanges();
					var selectedRooms = model.RoomsViewModel?
								   .Where(r => r.IsSelected)
								   .ToList();
					BookingDetails bookingDetails = _context.BookingDetails.Find(model.BookingID.Value);
					if (model.CreateEditAction == "Check-in")
					{	
						CommonCheckin(bookingDetails);
					}
					bookingDetails.AdvanceAmount = model.AdvanceAmount;
					bookingDetails.ModifiedBy = GetCurrentUserId();
					bookingDetails.ModifiedDate = DateTime.Now;
					_context.BookingDetails.Update(bookingDetails);
					_context.SaveChanges();

					_context.RoomDetails.RemoveRange(_context.RoomDetails.Where(rd => rd.BookingID == model.BookingID.Value));
					_context.SaveChanges();
					foreach (var room in selectedRooms)
					{
						RoomDetails roomDetails = new RoomDetails();
						roomDetails.BookingID = model.BookingID.Value;
						roomDetails.RoomID = room.RoomID;
						roomDetails.ExtraBeds = room.ExtraBeds;
						roomDetails.CreatedDate = DateTime.Now;
						roomDetails.CreatedBy = GetCurrentUserId();
						_context.RoomDetails.Add(roomDetails);
						_context.SaveChanges();
						// room.RoomTypeId
						// room.TotalRooms
						// room.ExtraBeds
					}
					transaction.Commit(); // Commit the transaction if all operations succeed
					return true;





					// Implement the logic to save booking details using the model
				}

				catch (Exception ex)
				{
					// Handle exceptions and rollback transaction if necessary
					transaction.Rollback();
					return false; // Indicate failure
				}
			}

		}
		public CustomerDetails GetCustomerDetails(int bookingID)
		{


			CustomerDetails customerDetails = new CustomerDetails();
			DataTable dt = _executeSP.ExecuteSPAsync("spCustomerDetails", new SqlParameter("@BookingID", bookingID)).Result;
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					customerDetails.Address1 = row["Address1"].ToString();
					customerDetails.Address2 = row["Address2"].ToString();
					customerDetails.City = row["City"].ToString();
					customerDetails.State = row["State"].ToString();
					customerDetails.PinCode = row["PinCode"].ToString();
					customerDetails.Email = row["Email"].ToString();
					customerDetails.PhoneNumber = row["PhoneNumber"].ToString();
					customerDetails.GST = row["GST"].ToString();
					customerDetails.AadharCardNumber = row["AadharCardNumber"].ToString();
					customerDetails.PanCardNumber = row["PanCardNumber"].ToString();
					customerDetails.DrivingLicenseNumber = row["DrivingLicenseNumber"].ToString();
					customerDetails.PassportNumber = row["PassportNumber"].ToString();
					customerDetails.Others = row["Others"].ToString();
					customerDetails.FirstName = row["FirstName"].ToString();
					customerDetails.LastName = row["LastName"].ToString();
					customerDetails.CustomerID = Convert.ToInt32(row["CustomerID"]);					
				}
			}
			return customerDetails;

		}
		public bool SaveBookingDetails(SearchViewModel model)
		{
			using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
			{
				try
				{
					CustomerDetails customerDetails = new CustomerDetails();
					customerDetails.FirstName = model.customerDetails?.FirstName;
					customerDetails.LastName = model.customerDetails?.LastName;
					customerDetails.Email = model.customerDetails?.Email;
					customerDetails.PhoneNumber = model.customerDetails?.PhoneNumber;
					customerDetails.Address1 = model.customerDetails?.Address1;
					customerDetails.Address2 = model.customerDetails?.Address2;
					customerDetails.GST = model.customerDetails?.GST;
					customerDetails.City = model.customerDetails?.City;
					customerDetails.State = model.customerDetails?.State;
					customerDetails.PinCode = model.customerDetails?.PinCode;
					customerDetails.AadharCardNumber = model.customerDetails?.AadharCardNumber;
					customerDetails.PanCardNumber = model.customerDetails?.PanCardNumber;
					customerDetails.DrivingLicenseNumber = model.customerDetails?.DrivingLicenseNumber;
					customerDetails.PassportNumber = model.customerDetails?.PassportNumber;
					customerDetails.Others = model.customerDetails?.Others;
					customerDetails.CreatedDate = DateTime.Now;
					customerDetails.CreatedBy = GetCurrentUserId(); // Replace with actual user if available					
					_context.CustomerDetails.Add(customerDetails);
					_context.SaveChanges();
					BookingDetails bookingDetails = new BookingDetails();
					bookingDetails.CutomerID = customerDetails.CustomerID;
					bookingDetails.AdvanceAmount = model.AdvanceAmount;
					bookingDetails.CheckInDate = model.CheckInDate.Value;
					bookingDetails.CheckOutDate = model.CheckOutDate.Value;
					bookingDetails.CreatedDate = DateTime.Now;
					bookingDetails.CreatedBy = GetCurrentUserId();
					bookingDetails.BookingStatus = "Booked";
					if(model.CreateEditAction == "Create Booking and Check-in")
					{
						CommonCheckin(bookingDetails);
					}
					_context.BookingDetails.Add(bookingDetails);
					_context.SaveChanges();

					var selectedRooms = model.RoomsViewModel?
						   .Where(r => r.IsSelected)
						   .ToList();
					foreach (var room in selectedRooms)
					{
						RoomDetails roomDetails = new RoomDetails();
						roomDetails.BookingID = bookingDetails.BookingID;
						roomDetails.RoomID = room.RoomID;
						roomDetails.ExtraBeds = room.ExtraBeds;
						roomDetails.CreatedDate = DateTime.Now;
						roomDetails.CreatedBy = GetCurrentUserId();
						_context.RoomDetails.Add(roomDetails);
						_context.SaveChanges();
						// room.RoomTypeId
						// room.TotalRooms
						// room.ExtraBeds
					}


					transaction.Commit(); // Commit the transaction if all operations succeed
					return true;





					// Implement the logic to save booking details using the model
				}

				catch (Exception ex)
				{
					// Handle exceptions and rollback transaction if necessary
					transaction.Rollback();
					return false; // Indicate failure
				}
			}
		}

		protected void CommonCheckin(BookingDetails bookingDetails)
		{
			bookingDetails.BookingStatus = "Checked-in";
			bookingDetails.CheckInTime = DateTime.Now;
		}
		public void SaveCheckOutBooking(SearchViewModel model)
		{
			BookingDetails bookingDetails = _context.BookingDetails.Find(model.BookingID.Value);
			CommonCheckOut(bookingDetails);
			bookingDetails.ModifiedBy=GetCurrentUserId();
			bookingDetails.ModifiedDate = DateTime.Now;
			_context.BookingDetails.Update(bookingDetails);
			_context.SaveChanges();

		}
		protected void CommonCheckOut(BookingDetails bookingDetails)
		{
			bookingDetails.BookingStatus = "Checked-out";
			bookingDetails.CheckOutTime = DateTime.Now;
		}
		public string? GetCurrentUserId()
		{
			return _httpContextAccessor.HttpContext?.User
				.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}
