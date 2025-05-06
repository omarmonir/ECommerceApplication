namespace eCommerceApp.Domain.Entities.Cart
{
    public class Achive
    {
        public Guid Id { get; set; } =Guid.NewGuid();
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
