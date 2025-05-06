using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Domain.Entities.Identity
{
	public class RefreshToken
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string UserId { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}
}
