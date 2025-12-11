namespace ArtBack.Domain.Entities;

public class DiscountCoupon : Entity
{
    public required string CouponCode { get; set; }
    public required string Description { get; set; }
    public required double DiscountAmount { get; set; }
    public required DateTime BeginAt { get; set; } = DateTime.UtcNow;
    public required DateTime ExpireAt { get; set; } = DateTime.UtcNow;
    public required bool IsActive { get; set; }
    public required decimal StartingPrice { get; set; }
    
    
    public ICollection<Order>? Orders { get; set; }
}