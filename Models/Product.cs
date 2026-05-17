

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
namespace EcommerceApp.Models
{
   
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be beween 3 and 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "Price mjst be greater than 0")]
        public double Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock must not be negative")]
        public int StockQuantity { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
       
        public int CategoryId { get; set; }
         public Category? Category { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    }

}
