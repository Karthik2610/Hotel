using HotelProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace HotelProject.Attributes
{
	//NodirectAccess
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class NoDirectAccessAttribute : Attribute, IFilterFactory
	{
		public bool IsReusable => false;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			// Resolve the filter through DI
			return serviceProvider.GetRequiredService<NoDirectAccessFilter>();
		}
	}
	public class NoDirectAccessFilter : IAsyncResourceFilter
	{





		private readonly ILoggerFactory _loggerFactory;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ILogger _logger;
		public NoDirectAccessFilter(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager, ILoggerFactory loggerFactory
			)
		{

			_userManager = userManager;
			_signInManager = signInManager;
			_loggerFactory = loggerFactory;
			_logger = loggerFactory.CreateLogger(GetType());
		}
		public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
		{
			_logger.LogInformation("Inside NoDirectAccessAttribute");
			try
			{
				var Action = "";
				//IdentityUser staff = new IdentityUser();
				ApplicationUser staff = new ApplicationUser();
				_logger.LogInformation("NoDirectAccessAttribute Before GetLoginUserId");

				var identity = context.HttpContext.User.Identity as ClaimsIdentity;

				if (identity == null || !identity.IsAuthenticated)
				{

					if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
					{
						_logger.LogInformation("NoDirectAccessAttribute identity.IsAuthenticated false - Ajax redirect");
						//filterContext.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
						context.Result = new JsonResult(new
						{
							error = "SESSION_EXPIRED"
						})
						{
							StatusCode = StatusCodes.Status401Unauthorized
						};
						return;
					}
					else
					{						
						_logger.LogInformation("NoDirectAccessAttribute identity.IsAuthenticated false - normal redirect");
						context.Result = new RedirectToRouteResult(new
															RouteValueDictionary
															{
															["area"] = "Identity",
															["page"] = "/Account/Login",
															//["ReturnUrl"] = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString
															});
								
						return;
					}
				}
				var resultContext5 = await next(); // Action and other filters execute

			}
			catch (Exception ex)
			{
				var InnerException = (ex.InnerException != null ? ex.InnerException.ToString() : "");
				System.Diagnostics.Trace.TraceError("Error Message " + ex.Message + " InnerException " + InnerException + " StackTrace " + ex.StackTrace);
				_logger.LogError("Error Message " + ex.Message + " InnerException " + InnerException + " StackTrace " + ex.StackTrace);
				throw ex;
			}

		}
	}
}
