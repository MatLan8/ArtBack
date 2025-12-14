namespace ArtBack.Domain.Dtos;

public class DiscountCouponVerificationDto
{
    public bool IsValid { get; init; }
    public string? ErrorCode { get; init; }
    public string? Message { get; init; }
    public double? DiscountValue { get; init; }
}