namespace ArtBack.Domain.Dtos;

public class DiscountCouponDto
{
    public Guid Id { get; set; }
    public string CouponCode { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double DiscountAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpireAt { get; set; }
    public DateTime BeginAt { get; set; }
    public decimal StartingPrice { get; set; }
}
