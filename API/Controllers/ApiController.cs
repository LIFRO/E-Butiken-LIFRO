using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers;

[ApiController]
[Route("api/Products")]
public class ProductApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductApiController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<ProductModel>>> GetAllProducts()
    {
        var products = await _db.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> GetProduct(int id)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductModel>> CreateProduct(ProductModel product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }
}