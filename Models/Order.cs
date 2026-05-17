

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
namespace EcommerceApp.Models
{
   
    public class Order
    {
        public int Id { get; set; }
       
         public double TotalAmount { get; set; }
         [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
       
        public int OrderItemsId { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        // public Category? Category { get; set; }
        
    }

}