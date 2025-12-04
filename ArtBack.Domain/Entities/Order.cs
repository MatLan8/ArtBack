namespace ArtBack.Domain.Entities;

public class Order: Entity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required double TotalSum { get; set; }
    public required string PaymentMethod { get; set; }
    public required string DeliveryAddress { get; set; }
    public required string DeliveryStatus { get; set; }
    public required DateTime DeliveryDate { get; set; }
    public required string TrackingNumber { get; set; }
    public required string Comment { get; set; }
    public required double AppliedDiscount { get; set; }
    public required string DeliveryMethod { get; set; }
    public Client Client { get; set; }
    public Guid ClientId { get; set; }
    public DiscountCoupon? DiscountCoupon { get; set; }
    public Guid? DiscountCouponId { get; set; }
    public ICollection<OrderArtwork> OrderArtwork { get; set; }
}