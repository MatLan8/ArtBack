namespace ArtBack.Domain.Dtos;

public class ValidateDiscountCouponDto
{
    public string CouponCode { get; set; } = null!;
    public double DiscountAmount { get; set; }
    public decimal StartingPrice { get; set; }
    public DateTime ExpireAt { get; set; }
}