using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace API.Models;

public class OrderItemModel
{
    public int Id { get; set; }

    [Required]
    public int OrderModelId { get; set; }
    [JsonIgnore]
    public OrderModel? Order { get; set; }

    [Required]
    public int ProductModelId { get; set; }
    public ProductModel? Product { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    public decimal LineTotal => (Product?.Price ?? 0) * Quantity;
}
