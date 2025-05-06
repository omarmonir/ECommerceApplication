using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Category
{
	public class CategoryBase
	{
		[Required]
		public string? Name { get; set; }

	}
}
