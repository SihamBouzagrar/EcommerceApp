

using System.ComponentModel.DataAnnotations;
namespace EcommerceApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
       public string Name { get; set; } = string.Empty;
       public ICollection<Product> Products { get; set; }
        
    }
}