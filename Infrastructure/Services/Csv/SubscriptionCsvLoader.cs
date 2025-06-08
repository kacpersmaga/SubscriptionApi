using Core.Entities;
using System.Globalization;

namespace Infrastructure.Services.Csv;

public class SubscriptionCsvLoader
{
    private readonly AppDbContext _context;

    public SubscriptionCsvLoader(AppDbContext context)
    {
        _context = context;
    }

    public async Task LoadFromFileAsync(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');

            if (parts.Length < 6)
                continue;

            var created = DateTime.ParseExact(parts[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);

            DateTime? canceled = null;
            if (!string.IsNullOrWhiteSpace(parts[2]))
                canceled = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var subscription = new Subscription
            {
                CustomerId = long.Parse(parts[0]),
                CreatedDate = created,
                CanceledDate = canceled,
                SubscriptionCost = int.Parse(parts[3]),
                SubscriptionInterval = parts[4],
                WasSubscriptionPaid = parts[5].Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase)
            };

            _context.Subscriptions.Add(subscription);
        }

        await _context.SaveChangesAsync();
    }
}