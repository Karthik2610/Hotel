using HotelProject.Attributes;
using HotelProject.Data;
using HotelProject.HelperClass;
using HotelProject.Models;
using HotelProject.Models.ViewModel;
using HotelProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Serilog;

namespace HotelProject.Controllers
{
	[NoDirectAccess]
	public class BookingController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly BookingService _bookingService;
		private readonly ILogger<BookingController> _logger;
		public BookingController(ApplicationDbContext context, BookingService bookingService, ILogger<BookingController> logger)
		{
			_context = context;
			_bookingService = bookingService;
			_logger = logger;
		}
		public IActionResult Index()
		{
			_logger.LogInformation("Index page loaded.");
			return View();
		}

		protected void CommonListLoad(SearchViewModel model)
		{
			DataTable dataTable = _bookingService.RoomDetails(model.CheckInDate.Value, model.CheckOutDate.Value);
			model.ListModel = dataTable;
		}
		protected void LoadRooms(SearchViewModel model)
		{
			model.RoomsViewModel = new List<RoomsViewModel>();
			_context.RoomsLU.ToList().ForEach(x => model.RoomsViewModel?.Add(new RoomsViewModel { RoomID = x.RoomID, RoomName = x.RoomName, TotalRooms = x.TotalRooms }));

		}



		[HttpPost]
		public IActionResult Index(SearchViewModel model, string action)
		{
			if (model.CheckOutDate <= model.CheckInDate)
			{
				ModelState.AddModelError(nameof(model.CheckOutDate),
					"Check-out date must be greater than check-in date.");
				return View(model);
			}

			if (action == "Search")
			{
				if (ModelState["CheckInDate"]?.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
	ModelState["CheckOutDate"]?.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
				{
					CommonListLoad(model);
					model.CreateEditAction = "Create Booking";
					if (model.ListModel?.Rows.Count > 0)
					{
						var bookedRoomIds = model.ListModel.AsEnumerable()
										.Select(r => r.Field<int>("RoomID"))
										.ToList();
						model.RoomsViewModel = _context.RoomsLU
							.Where(r => !bookedRoomIds.Contains(r.RoomID))
							.Select(r => new RoomsViewModel
							{
								RoomID = r.RoomID,
								RoomName = r.RoomName,
								TotalRooms = r.TotalRooms
							})
							.ToList();

						var bookedCounts = model.ListModel.AsEnumerable()
						.GroupBy(r => r.Field<int>("RoomID"))
						.ToDictionary(
							g => g.Key,
							g => g.Count());

						bool roomsAvailable = _context.RoomsLU.ToList().Any(r =>
							!bookedCounts.ContainsKey(r.RoomID));

						bool AvailableRooms = roomsAvailable
							? true
							: false;
						if (!AvailableRooms)
						{
							model.CreateEditAction = string.Empty;
						}
					}
					else
					{
						LoadRooms(model);
					}
				}
				else
				{
					if (ModelState["CheckInDate"]?.ValidationState != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
					{
						ModelState.AddModelError("CheckInDate", "Check-in date is required.");
					}
					if (ModelState["CheckOutDate"]?.ValidationState != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
					{
						ModelState.AddModelError("CheckOutDate", "Check-out date is required.");
					}

				}


				model.SearchAction = "Search";


			}
			else if (action == "Create Booking")
			{
				model.CreateEditAction = "Create Booking";
				CommonCreateBooking(model);
			}
			else if (action == "Create Booking and Check-in")
			{
				model.CreateEditAction = "Create Booking and Check-in";
				CommonCreateBooking(model);
				model.CreateEditAction = "Create Booking";
			}
			if (model.ListModel.Columns.Contains("RoomID"))
			{
				model.ListModel.Columns.Remove("RoomID");
			}
			return View(model);
		}
		protected void CommonCreateBooking(SearchViewModel model)
		{
			if (ModelState.IsValid)
			{
				CommonListLoad(model);
				if (model.RoomsViewModel.Count > 0)
				{
					int roomCounter= model.RoomsViewModel.Count(x => x.IsSelected == true);
					if(roomCounter <= 0)
					{
						TempData["ErrorMessage"] = "At least one room must be selected .";
						model.SearchAction = "Search";
						return;
					}
					
				}
				
				bool status = _bookingService.SaveBookingDetails(model);

				if (status)
				{
					TempData["SuccessMessage"] = "Booking details saved successfully.";
					model.CreateEditAction = string.Empty;
					model.SearchAction = "Search";
					CommonListLoad(model);
				}
				else
				{
					TempData["ErrorMessage"] = "Failed to save booking details.";
				}
			}
			else
			{
				CommonListLoad(model);
				model.SearchAction = "Search";
			}
		}	
		public IActionResult Edit(int id)
		{
			SearchViewModel model = new SearchViewModel();

			model.customerDetails = _bookingService.GetCustomerDetails(id);
			BookingDetails bookingDetails = _bookingService.GetBookingDetails(id);
			model.AdvanceAmount = bookingDetails.AdvanceAmount;
			model.CheckInDate = bookingDetails.CheckInDate;
			model.CheckOutDate = bookingDetails.CheckOutDate;
			model.BookingID = id;
			CommonListLoad(model);
			DataTable roomsForBookingId = _bookingService.GetRoomsForBooking(id, bookingDetails.CheckInDate, bookingDetails.CheckOutDate);

			//var bookedRoomForBookingId = roomsForBookingId.AsEnumerable()
			//							.Select(r => r.Field<int>("RoomID"))
			//							.ToList();

			//var bookedRoomIds = model.ListModel.AsEnumerable()
			//							.Select(r => r.Field<int>("RoomID"))
			//							.ToList();
			////For that date range, get all rooms and get not booked rooms 
			//List<RoomsViewModel> availableRooms = _context.RoomsLU
			//				.Where(r => !bookedRoomIds.Contains(r.RoomID))
			//				.Select(r => new RoomsViewModel
			//				{
			//					RoomID = r.RoomID,
			//					RoomName = r.RoomName,
			//					TotalRooms = r.TotalRooms
			//				})
			//				.ToList();

			////Get

			//List<RoomsViewModel> bookedRooms = _context.RoomsLU
			//	.Where(r => bookedRoomForBookingId.Contains(r.RoomID))
			//	.Select(r => new RoomsViewModel
			//	{
			//		RoomID = r.RoomID,
			//		RoomName = r.RoomName,
			//		TotalRooms = r.TotalRooms,
			//		IsSelected = true
			//	})
			//	.ToList();

			//var allRooms = availableRooms.ToDictionary(r => r.RoomID);

			//foreach (var booked in bookedRooms)
			//{
			//	if (allRooms.TryGetValue(booked.RoomID, out var room))
			//	{
			//		room.IsSelected = true;
			//	}
			//	else
			//	{
			//		booked.IsSelected = true;
			//		allRooms.Add(booked.RoomID, booked);
			//	}
			//}
			if (bookingDetails.BookingStatus == "Checked-out")
			{
				model.CreateEditAction = "HideButtons";
				model.RoomsViewModel = roomsForBookingId.AsEnumerable().Where(t=>t.Field<bool>("IsSelected")==true).Select(r => new RoomsViewModel
				{
					RoomID = r.Field<int>("RoomID"),
					RoomName = r.Field<string>("RoomName"),
					IsSelected = r.Field<bool>("IsSelected"),
					ExtraBeds = r.Field<int>("ExtraBeds")
				})
				.ToList(); 
			}
			else
			{

				model.RoomsViewModel = roomsForBookingId.AsEnumerable().Select(r => new RoomsViewModel
				{
					RoomID = r.Field<int>("RoomID"),
					RoomName = r.Field<string>("RoomName"),
					IsSelected = r.Field<bool>("IsSelected"),
					ExtraBeds = r.Field<int>("ExtraBeds")
				})
				.ToList();
			}

			return View(model);
		}
		[HttpPost]
		public IActionResult Edit(SearchViewModel model, string action)
		{
			if (action == "Cancel Booking")
			{
				bool returnStatus = _bookingService.CancelBooking(model);
				if (returnStatus)
				{
					TempData["SuccessMessage"] = "Booking cancelled successfully.";
				}
				else
				{
					TempData["ErrorMessage"] = "Failed to cancel booking.";
				}
			}
			else if (action == "Edit")
			{
				bool returnStatus = _bookingService.SaveCustomerDetails(model);
				if (returnStatus)
				{
					TempData["SuccessMessage"] = "Customer details saved successfully.";
				}
				else
				{
					TempData["ErrorMessage"] = "Failed to save customer details.";
				}
			}
			else if (action == "Check-in")
			{
				model.CreateEditAction = "Check-in";
				bool returnStatus = _bookingService.SaveCustomerDetails(model);
				if (returnStatus)
				{
					TempData["SuccessMessage"] = "Customer details saved successfully.";
				}
				else
				{
					TempData["ErrorMessage"] = "Failed to save customer details.";
				}
			}
			return RedirectToActionPermanent("Index");
		}

		[HttpPost]
		public IActionResult CheckOut(SearchViewModel model, string action)
		{
			CheckOutViewModel checkOutViewModel = new CheckOutViewModel();
			_bookingService.SaveCheckOutBooking(model);
			HotelDetails hotelDetails = _context.HotelDetails.Find(1);//1 is the hotel ID
			CustomerDetails customerDetails= _context.CustomerDetails.Find(model.customerDetails.CustomerID);
			checkOutViewModel.HotelDetails = hotelDetails;
			checkOutViewModel.CustomerDetails = customerDetails;
			checkOutViewModel.RoomDetails = _bookingService.CheckOutDetails(model.BookingID.Value);
			checkOutViewModel.BookingID = model.BookingID;
			decimal totalAmount = 0;
			foreach (DataRow dr in checkOutViewModel.RoomDetails.Rows)
			{
				totalAmount = Convert.ToDecimal(dr["TotalAmount"]);
				
			}
			checkOutViewModel.AmountInWords = Helper.NumberToWordsIndian(totalAmount);
			return View(checkOutViewModel);
		}
	}
}
