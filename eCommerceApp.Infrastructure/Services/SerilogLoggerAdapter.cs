using eCommerceApp.Application.Services.Interfaces.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Infrastructure.Services
{
	public class SerilogLoggerAdapter<T>(ILogger<T> logger) : IAppLogger<T>
	{


		public void LogError(Exception ex, string message) => logger.LogError(ex, message);
		public void LogInformation(string message) => logger.LogInformation(message);
		public void LogWaning(string message) => logger.LogWarning(message);
		
	}
}
