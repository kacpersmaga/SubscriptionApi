using Infrastructure;
using Infrastructure.Services.Csv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=subscriptions.db"));

builder.Services.AddScoped<SubscriptionCsvLoader>();

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

    if (!db.Subscriptions.Any())
    {
        var loader = scope.ServiceProvider.GetRequiredService<SubscriptionCsvLoader>();
        var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "subscription.csv");
        if (File.Exists(csvPath))
            await loader.LoadFromFileAsync(csvPath);
    }
}

app.MapControllers();
app.MapGet("/", () => "Subscription API running...");

app.Run();