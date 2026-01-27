using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace API.Models;

public class ProductModel
{
    public int Id { get; set; }

    [Required, MaxLength(20) ]
    public string ProductId { get; set; }= string.Empty;

    [Required, MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set;}

    [Required]
    public int Stock { get; set; }
}