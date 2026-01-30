using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace API.Models;

public class ProductModel
{
    public int Id { get; set; }

    [MaxLength(20)]
    public string? Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set;}

    [Required]
    public int Stock { get; set; }
}
