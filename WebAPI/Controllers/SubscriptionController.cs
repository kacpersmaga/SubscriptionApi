using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.DTOs.Subscription;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubscriptionController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
    {
        return await _context.Subscriptions.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetSubscription(int id)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subscription == null)
        {
            return NotFound();
        }

        return subscription;
    }
    
    [HttpGet("{id}/details")]
    public async Task<ActionResult<object>> GetSubscriptionWithCustomer(int id)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subscription == null)
        {
            return NotFound();
        }

        var customer = await _context.Customers
            .Where(c => c.Id == subscription.CustomerId)
            .Select(c => new {
                c.Id,
                c.FirstName,
                c.LastName,
                c.Email,
                c.CreatedDate
            })
            .FirstOrDefaultAsync();

        return Ok(new {
            Subscription = subscription,
            Customer = customer
        });
    }
    
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptionsByCustomer(long customerId)
    {
        var subscriptions = await _context.Subscriptions
            .Where(s => s.CustomerId == customerId)
            .ToListAsync();

        return subscriptions;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Subscription>> CreateSubscription(SubscriptionCreateDto subscriptionDto)
    {
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == subscriptionDto.CustomerId);
        if (!customerExists)
        {
            return BadRequest("Customer not found");
        }

        var subscription = new Subscription
        {
            CustomerId = subscriptionDto.CustomerId,
            CreatedDate = subscriptionDto.CreatedDate,
            CanceledDate = subscriptionDto.CanceledDate,
            SubscriptionCost = subscriptionDto.SubscriptionCost,
            SubscriptionInterval = subscriptionDto.SubscriptionInterval,
            WasSubscriptionPaid = subscriptionDto.WasSubscriptionPaid
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSubscription), new { id = subscription.Id }, subscription);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubscription(int id, SubscriptionUpdateDto subscriptionDto)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        
        if (subscription == null)
        {
            return NotFound();
        }
        
        if (subscriptionDto.CustomerId != subscription.CustomerId)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == subscriptionDto.CustomerId);
            if (!customerExists)
            {
                return BadRequest("Customer not found");
            }
        }

        subscription.CustomerId = subscriptionDto.CustomerId;
        subscription.CanceledDate = subscriptionDto.CanceledDate;
        subscription.SubscriptionCost = subscriptionDto.SubscriptionCost;
        subscription.SubscriptionInterval = subscriptionDto.SubscriptionInterval;
        subscription.WasSubscriptionPaid = subscriptionDto.WasSubscriptionPaid;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubscriptionExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        
        if (subscription == null)
        {
            return NotFound();
        }

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SubscriptionExists(int id)
    {
        return _context.Subscriptions.Any(e => e.Id == id);
    }
}