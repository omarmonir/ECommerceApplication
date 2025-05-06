using eCommerceApp.Domain.Entities.Cart;
using eCommerceApp.Domain.Interfaces.Cart;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Repositories.Cart
{
    public class CartRepository(AppDbContext context) : ICart
    {
        public async Task<IEnumerable<Achive>> GetAllCheckoutHistory()
        {
           return await context.CheckoutAchives.AsNoTracking().ToListAsync();   
        }

        public async Task<int> SaveCheckoutHistory(IEnumerable<Achive> checkouts)
        {
            context.CheckoutAchives.AddRange(checkouts);
            return await context.SaveChangesAsync();
        }
    }
}
