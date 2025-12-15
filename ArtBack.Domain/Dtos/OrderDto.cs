namespace ArtBack.Domain.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalSum { get; set; }
    public string DeliveryStatus { get; set; }
}