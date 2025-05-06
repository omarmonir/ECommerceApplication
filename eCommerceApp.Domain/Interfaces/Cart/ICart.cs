using eCommerceApp.Domain.Entities.Cart;

namespace eCommerceApp.Domain.Interfaces.Cart
{
    public interface ICart
    {
        Task<int> SaveCheckoutHistory(IEnumerable<Achive> checkouts);
        Task<IEnumerable<Achive>> GetAllCheckoutHistory();
    }
}
