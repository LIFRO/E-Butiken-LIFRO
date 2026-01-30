using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace API.Models;

// Represents a customer order with line items.
public class OrderModel
{
    public int Id { get; set; }

    // FK to the user who placed the order
    [Required]
    public int AppUserId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation collection of items in the order
    public List<OrderItemModel> Items { get; set; } = new();

    // Convenience total; can be computed from Items if preferred
    public decimal Total => Items.Sum(i => i.LineTotal);
}
