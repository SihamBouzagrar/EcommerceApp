
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
[Required]
        public int OrderId { get; set; }
            public Order? Order{ get; set; } 
            [Required]
         public int ProductId { get; set; }
          public Product? Product { get; set; }
          public int Quantity { get; set; }
           public double UnitPrice { get; set; }
      
        
    }
}