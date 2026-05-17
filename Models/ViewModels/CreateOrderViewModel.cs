using System.ComponentModel.DataAnnotations;
namespace ECommerceApp.ViewModels
{
public class CreateOrderViewModel
{
[Required]
[Range(1, int.MaxValue, ErrorMessage = "Please select a product.")]
public int ProductId { get; set; }
[Required]
[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]

public int Quantity { get; set; }
}
}