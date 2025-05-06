using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Entities.Cart;
using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Domain.Interfaces.Cart;

namespace eCommerceApp.Application.Services.Implementations.Cart
{
    public class CartService(ICart cartInterface, IMapper mapper, IGeneric<Product> productInterface,
        IPaymentMethodService paymentMethodService, IPaymentService paymentService, IUserMangement userMangement) : ICartService
    {
        public async Task<ServiceResponse> Checkout(CheckoutDto checkout)
        {
            var (products, totalAmount) = await GetCartTotalAmount(checkout.Carts);
            var pymentMethod = await paymentMethodService.GetPaymentMethods();

            if(checkout.PaymentMethodId == pymentMethod.FirstOrDefault().Id)
            {
                return await paymentService.Pay(totalAmount, products, checkout.Carts);
            }
            return new ServiceResponse(false, "Invalid payment method");

        }

        public async Task<IEnumerable<GetAchieve>> GetAchieves()
        {
           var history = await cartInterface.GetAllCheckoutHistory();
            if (history == null) return [];
            var groupByCustomerId = history.GroupBy(x => x.UserId).ToList();
            var products = await productInterface.GetAllAsync();
            var achieves = new List<GetAchieve>();
            foreach (var customerId in groupByCustomerId)
            {
                var customerDetails = await userMangement.GetUserById<AppUser>(customerId.Key!);
                foreach (var item in customerId)
                {
                    var product = products.FirstOrDefault(x => x.Id == item.ProductId);
                    achieves.Add(new GetAchieve
                    {
                        CustomerName = customerDetails.FullName,
                        CustomerEmail = customerDetails.Email,
                        ProductName = product!.Name,
                        AmountPayed = item.Quantity * product.Price,
                        QuantityOrder = item.Quantity,
                        DatePurchased = item.CreatedDate

                    });
                }
               
            }
            return achieves;
        }

        public async Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateAchiveDto> achives)
        {
           var mappedData = mapper.Map<IEnumerable<Achive>>(achives);
           var result = await cartInterface.SaveCheckoutHistory(mappedData);
            return result > 0 ? new ServiceResponse(true, "Checkout Achived") :
                new ServiceResponse(false, "Error occurred in saving");
        }
        private async Task<(IEnumerable<Product>, decimal)> GetCartTotalAmount(IEnumerable<ProcessCart> carts)
        {
            if (!carts.Any()) return ([], 0);

            var products = await productInterface.GetAllAsync();
            if(!products.Any()) return ([], 0);

            var cartProducts = carts
                .Select(cartItem => products.FirstOrDefault(p => p.Id == cartItem.ProductId))
                .Where(product => product != null)
                .ToList();

            var totalAmount = carts
                .Where(cartItem => cartProducts.Any(p => p.Id == cartItem.ProductId))
                .Sum(cartItem => cartItem.Quantity * cartProducts.First(p => p.Id == cartItem.ProductId)!.Price);

            return(cartProducts!, totalAmount);
                
        }
    }
}
