using eCommerceApp.Domain.Entities.Cart;

namespace eCommerceApp.Domain.Interfaces.Cart
{
    public interface IPaymentMethod
    {
        Task<IEnumerable<PaymentMetod>> GetPaymentAsync();
    }
}
