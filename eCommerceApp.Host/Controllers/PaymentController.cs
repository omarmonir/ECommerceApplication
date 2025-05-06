using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Mvc;
namespace eCommerceApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentMethodService paymentMethodService) : ControllerBase
    {
        [HttpGet("methods")]
        public async Task<ActionResult<IEnumerable<GetPaymentMethod>>> GetPaymentMethods()
        {
            var methods = await paymentMethodService.GetPaymentMethods();
            if(!methods.Any()) 
                return NotFound();
            return Ok(methods);
        }
    }
}
