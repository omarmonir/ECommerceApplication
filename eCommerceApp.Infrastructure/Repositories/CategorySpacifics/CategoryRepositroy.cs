using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces.CategorySpacifics;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Repositories.CategorySpacifics
{
    public class CategoryRepositroy(AppDbContext context): ICategory
    {
        public async Task<IEnumerable<Product>> GetProductsByCategory(Guid ctegoryId)
        {
            var products = await context
                 .Products
                 .Include(x => x.Category)
                 .Where(x => x.CategoryId == ctegoryId)
                 .AsNoTracking()
                 .ToListAsync();
            return products.Count > 0 ? products : [];
        }
    }
}
