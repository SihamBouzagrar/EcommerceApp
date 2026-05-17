using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs
{
public class ProductUpdateDto
{
[Required]
public int Id { get; set; }
[Required]
[StringLength(100, MinimumLength = 3)]
public string Name { get; set; } = string.Empty;
[StringLength(500)]
public string? Description { get; set; }
[Range(0.01, 1000000)]
public double Price { get; set; }
[Range(0, int.MaxValue)]
public int StockQuantity { get; set; }
[Required]
public int CategoryId { get; set; }
}
}