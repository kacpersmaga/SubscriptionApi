using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Infrastructure.Services.Csv;

public class CustomerCsvLoader
{
    private readonly AppDbContext _context;

    public CustomerCsvLoader(AppDbContext context)
    {
        _context = context;
    }

    public async Task LoadSampleDataAsync()
    {
        if (!await _context.Customers.AnyAsync())
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@email.com", CreatedDate = DateTime.Parse("2023-01-15") },
                new Customer { Id = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@email.com", CreatedDate = DateTime.Parse("2023-02-20") },
                new Customer { Id = 3, FirstName = "Piotr", LastName = "Wiśniewski", Email = "piotr.wisniewski@email.com", CreatedDate = DateTime.Parse("2023-03-10") },
                new Customer { Id = 4, FirstName = "Maria", LastName = "Wójcik", Email = "maria.wojcik@email.com", CreatedDate = DateTime.Parse("2023-04-05") },
                new Customer { Id = 5, FirstName = "Tomasz", LastName = "Kowalczyk", Email = "tomasz.kowalczyk@email.com", CreatedDate = DateTime.Parse("2023-05-12") }
            };

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateCustomersFromSubscriptionCsvAsync(string subscriptionCsvPath)
    {
        if (!File.Exists(subscriptionCsvPath))
            return;

        var lines = await File.ReadAllLinesAsync(subscriptionCsvPath);
        var uniqueCustomerIds = new HashSet<long>();
        
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');
            if (parts.Length >= 1 && long.TryParse(parts[0], out var customerId))
            {
                uniqueCustomerIds.Add(customerId);
            }
        }
        
        var existingCustomerIds = await _context.Customers
            .Where(c => uniqueCustomerIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();
        
        var missingCustomerIds = uniqueCustomerIds.Except(existingCustomerIds);
        var customersToAdd = new List<Customer>();

        foreach (var customerId in missingCustomerIds)
        {
            customersToAdd.Add(new Customer
            {
                Id = customerId,
                FirstName = $"Customer{customerId}",
                LastName = "Generated",
                Email = $"customer{customerId}@generated.com",
                CreatedDate = DateTime.UtcNow
            });
        }

        if (customersToAdd.Any())
        {
            await _context.Customers.AddRangeAsync(customersToAdd);
            await _context.SaveChangesAsync();
        }
    }

    public async Task LoadFromFileAsync(string path)
    {
        if (!File.Exists(path))
            return;

        var lines = await File.ReadAllLinesAsync(path);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');

            if (parts.Length < 4)
                continue;

            var customer = new Customer
            {
                Id = long.Parse(parts[0]),
                FirstName = parts[1].Trim('"'),
                LastName = parts[2].Trim('"'),
                Email = parts[3].Trim('"'),
                CreatedDate = DateTime.ParseExact(parts[4], "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
            
            if (!await _context.Customers.AnyAsync(c => c.Id == customer.Id))
            {
                _context.Customers.Add(customer);
            }
        }

        await _context.SaveChangesAsync();
    }
}