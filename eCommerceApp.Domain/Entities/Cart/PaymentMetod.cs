using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Domain.Entities.Cart
{
    public class PaymentMetod
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
