using eCommerceApp.Domain.Entities.Cart;
using eCommerceApp.Domain.Interfaces.Cart;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace eCommerceApp.Infrastructure.Repositories.Cart
{
    public class PaymentMethodRepository(AppDbContext context) : IPaymentMethod
    {
        public async Task<IEnumerable<PaymentMetod>> GetPaymentAsync()
        {
            return await context.PaymentMetods.AsNoTracking().ToListAsync();
        }
    }
}
