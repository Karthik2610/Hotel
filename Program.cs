using HotelProject.Attributes;
using HotelProject.Data;
using HotelProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	//.WriteTo.File(
	//	"..\\Logs\\log-.txt",		
	//	retainedFileCountLimit: 10,
	//	rollOnFileSizeLimit: true,
	//	fileSizeLimitBytes: 1000000,
	//	shared: true)
	.CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// Remove default Microsoft logging providers
builder.Logging.ClearProviders();
builder.Host.UseSerilog();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<ExecuteSP>();
//builder.Services.AddRazorPages(options =>
//{
//	options.Conventions.AddAreaPageApplicationModelConvention("Identity", "/Account/Login", model =>
//	{
//		model.Filters.Add(new NoDirectAccessAttribute());
//	});
//});
builder.Services.AddScoped<NoDirectAccessFilter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Booking}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
