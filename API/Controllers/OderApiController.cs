using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers;

[ApiController]
[Route("api/Orders")]
public class OderApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public OderApiController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<OrderModel>> CreateOrder([FromBody] CreateOrderDto request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserMail == request.user);
        if (user == null)
        {
            return NotFound($"User '{request.user}' not found");
        }

        // get all requested product ids in one query
        var productIds = request.Products.Select(i => i.id).Distinct().ToList();
        var products = await _db.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        if (products.Count != productIds.Count)
            return BadRequest("One or more products do not exist.");

        foreach(ProductModel product in products)
        {
            foreach(OrderItemDto item in request.Products)
            {
            
            if(item.id != product.Id) continue;

            if(product.Stock < item.Quantity)
                {
                    return BadRequest($"Not enough stock for product {product.Name}.");
                }

                    product.Stock -= item.Quantity;
                    break;
            }
        }

        var order = new OrderModel
        {
            AppUserId = user.id,
        
            Items = request.Products.Select(i => new OrderItemModel
            {
                ProductModelId = i.id,
                Quantity = i.Quantity
            }).ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return Created($"/api/orders/{order.Id}", order);
    }
    
    [HttpGet]
    public async Task<ActionResult<OrderModel>> GetOrder()
    {
        var orders = await _db.Orders
        .Include(o => o.Items)
        .ThenInclude(i => i.Product)
        .ToListAsync();

        return Ok(orders);
    }
}

// DTOs for order creation
public class CreateOrderDto
{
    public string user { get; set; } = string.Empty;
    public List<OrderItemDto> Products { get; set; } = new();
}

public class OrderItemDto
{
    public int id { get; set; }
    public int Quantity { get; set; }
}
