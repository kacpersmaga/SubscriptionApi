using System.Text.Json.Serialization;

namespace Core.Entities;

public class Customer
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    [JsonIgnore]
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}