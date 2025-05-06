using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Product;

namespace eCommerceApp.Application.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<GetCategory>> GetAllAsync();

		Task<GetCategory> GetByIdAsync(Guid id);
		Task<ServiceResponse> AddAsync(CreateCategory category);

		Task<ServiceResponse> UpdateAsync(UpdateCategory category);

		Task<ServiceResponse> DeleteAsync(Guid id);
		Task<IEnumerable<GetProduct>> GetProductByCategory(Guid categoryId);
	}
}
