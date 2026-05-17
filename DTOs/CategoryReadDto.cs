namespace ECommerceApp.DTOs
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Optional: number of products in this category
        public int ProductCount { get; set; }
    }
}