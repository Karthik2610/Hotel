using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelProject.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<HotelProject.Models.CustomerDetails> CustomerDetails { get; set; } = default!;
		public DbSet<HotelProject.Models.HotelDetails> HotelDetails { get; set; } = default!;
		public DbSet<HotelProject.Models.RoomsLU> RoomsLU { get; set; } = default!;
		public DbSet<HotelProject.Models.BookingDetails> BookingDetails { get; set; } = default!;
		public DbSet<HotelProject.Models.RoomDetails> RoomDetails { get; set; } = default!;
		public ApplicationDbContext(
			DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	}
}
