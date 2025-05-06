using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Product
{
	public class UpdateProduct: ProductBase
	{

		[Required]
		public Guid Id { get; set; }
	}
}
