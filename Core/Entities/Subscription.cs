﻿using System.Text.Json.Serialization;

namespace Core.Entities;

public class Subscription
{
    public int Id { get; set; }
    public long CustomerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? CanceledDate { get; set; }
    public int SubscriptionCost { get; set; }
    public string SubscriptionInterval { get; set; } = null!;
    public bool WasSubscriptionPaid { get; set; }
    
    [JsonIgnore]
    public Customer Customer { get; set; } = null!;
}