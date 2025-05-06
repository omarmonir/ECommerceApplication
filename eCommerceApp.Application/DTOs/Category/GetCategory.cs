using eCommerceApp.Application.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Category
{
	public class GetCategory : CategoryBase
	{
		public Guid Id { get; set; }
		public ICollection<GetProduct>? Products { get; set; }
	}
}
