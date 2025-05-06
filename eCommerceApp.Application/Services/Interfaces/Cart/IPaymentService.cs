using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Domain.Entities;
namespace eCommerceApp.Application.Services.Interfaces.Cart
{
    public interface IPaymentService
    {
        Task<ServiceResponse> Pay(decimal totalAmount, IEnumerable<Product> cartProducts, IEnumerable<ProcessCart> carts);
    }
}
