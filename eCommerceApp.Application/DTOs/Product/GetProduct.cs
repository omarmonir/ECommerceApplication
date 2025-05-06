using eCommerceApp.Application.DTOs.Category;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Product
{
	public class GetProduct : ProductBase
	{

		[Required]
		public Guid Id { get; set; }
		public GetCategory? Category { get; set; } = new();
        public DateTime CreatedDate { get; set; }
    }
}
