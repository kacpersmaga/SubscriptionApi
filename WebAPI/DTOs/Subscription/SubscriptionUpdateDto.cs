namespace WebAPI.DTOs.Subscription;

public class SubscriptionUpdateDto
{
    public long CustomerId { get; set; }
    public DateTime? CanceledDate { get; set; }
    public int SubscriptionCost { get; set; }
    public string SubscriptionInterval { get; set; } = null!;
    public bool WasSubscriptionPaid { get; set; }
}