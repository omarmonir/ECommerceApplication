  using eCommerceApp.Application.Services.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace eCommerceApp.Infrastructure.Middleware
{
	public class ExceptionHandlingMiddleware(RequestDelegate _next)
	{
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (DbUpdateException ex)
			{
				var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
				context.Response.ContentType = "application/json";	
				if (ex.InnerException is SqlException innerException)
				{
					logger.LogError(innerException, "Sql Exception");
					switch (innerException.Number)
					{
						case 2627: // Unique constraint violation
							context.Response.StatusCode = StatusCodes.Status409Conflict;
							await context.Response.WriteAsync("Unique constraint violation");
							break;
						case 515: // Cannot insert null
							context.Response.StatusCode = StatusCodes.Status400BadRequest;
							await context.Response.WriteAsync("Cannot insert null");
							break;
						case 547: // Foreign Key constraint violation
							context.Response.StatusCode = StatusCodes.Status409Conflict;
							await context.Response.WriteAsync("Foreign Key constraint violation");
							break;
						default:
							context.Response.StatusCode = StatusCodes.Status500InternalServerError;
							await context.Response.WriteAsync("An error occurred while saving the entity changes.");
							break;
					}
				}

				else
				{
					logger.LogError(ex, "Related EFCore Exception");

					context.Response.StatusCode = StatusCodes.Status500InternalServerError;
					await context.Response.WriteAsync("An error occurred while saving the entity changes.");
				}
			}
			catch (Exception ex)
			{
				var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
				logger.LogError(ex, "UnKnown Exception");
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsync("An error occurred" + ex.Message);
			}
		}
	}
}
