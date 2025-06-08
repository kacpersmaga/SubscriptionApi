namespace Core.Entities;

public class Subscription
{
    public int Id { get; set; }
    public string Country { get; set; } = null!;
    public string Product { get; set; } = null!;
    public string Plan { get; set; } = null!;
    public DateTime SubscriptionStartDate { get; set; }
    public int MonthlyRevenue { get; set; }
}