using System.Globalization;
using Core.Entities;

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
            var subscription = new Subscription
            {
                Country = parts[0],
                Product = parts[1],
                Plan = parts[2],
                SubscriptionStartDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                MonthlyRevenue = int.Parse(parts[4])
            };
            _context.Subscriptions.Add(subscription);
        }

        await _context.SaveChangesAsync();
    }
}