using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}