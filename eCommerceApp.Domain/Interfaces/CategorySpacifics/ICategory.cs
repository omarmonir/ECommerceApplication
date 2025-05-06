using eCommerceApp.Domain.Entities;

namespace eCommerceApp.Domain.Interfaces.CategorySpacifics
{
    public interface ICategory
    {
        Task<IEnumerable<Product>> GetProductsByCategory(Guid ctegoryId);

    }
}
