using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/")]
public class ProductApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductApiController(AppDbContext db) => _db = db;

    [HttpGet("products")]
    public async Task<ActionResult<List<ProductModel>>> GetAllProducts()
    {
        var products = await _db.Products.ToListAsync();
        return Ok(products);
    }
}
