using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Domain.Entities.Cart;

namespace eCommerceApp.Application.Services.Interfaces.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateAchiveDto> achives);
        Task<ServiceResponse> Checkout(CheckoutDto checkout);
        Task<IEnumerable<GetAchieve>> GetAchieves();
    }
}
