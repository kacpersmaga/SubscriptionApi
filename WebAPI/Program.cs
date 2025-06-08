using Infrastructure;
using Infrastructure.Services.Csv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=subscriptions.db"));

builder.Services.AddScoped<SubscriptionCsvLoader>();
builder.Services.AddScoped<CustomerCsvLoader>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "subscription.csv");
    
    var customerLoader = scope.ServiceProvider.GetRequiredService<CustomerCsvLoader>();
    
    if (File.Exists(csvPath))
    {
        await customerLoader.CreateCustomersFromSubscriptionCsvAsync(csvPath);
    }
    else
    {
        await customerLoader.LoadSampleDataAsync();
    }
    
    if (!db.Subscriptions.Any() && File.Exists(csvPath))
    {
        var subscriptionLoader = scope.ServiceProvider.GetRequiredService<SubscriptionCsvLoader>();
        await subscriptionLoader.LoadFromFileAsync(csvPath);
    }
}

app.MapControllers();
app.MapGet("/", () => "Subscription API running...");

app.Run();