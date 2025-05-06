using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Services.Interfaces.Logging
{
	public interface IAppLogger<T>
	{
		void LogInformation(string message);
		void LogWaning(string message);
		void LogError(Exception ex,string message);
	}
}
