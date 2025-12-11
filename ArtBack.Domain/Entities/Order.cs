using ArtBack.Domain.Types;

namespace ArtBack.Domain.Entities;

public class Order: Entity
{
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required decimal TotalSum { get; set; }
    public required string PaymentMethod { get; set; }
    public required string DeliveryAddress { get; set; }
    public required string DeliveryStatus { get; set; }
    public required DateTime DeliveryDate { get; set; }
    public required string TrackingNumber { get; set; }
    public required string Comment { get; set; }
    public decimal? AppliedDiscount { get; set; }
    public required DeliveryMethod DeliveryMethod { get; set; }
    
    public required Guid ClientId { get; set; }
    public Guid? DiscountCouponId { get; set; }
    
    public Client? Client { get; set; }
    public DiscountCoupon? DiscountCoupon { get; set; }
    public ICollection<OrderArtwork> OrderArtwork { get; set; }
}