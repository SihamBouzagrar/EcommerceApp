using Microsoft.EntityFrameworkCore;

using EcommerceApp.Models;
namespace ECommerceApp.Data
{
 public class ApplicationDbContext : DbContext
 {
 // Constructor with options passed from Program.cs
 public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
 : base(options)
 { }
 // Represents the Products table in the database
 public DbSet<Product> Products { get; set; }

 public DbSet<Category> Categories { get; set; }
  public DbSet<Order> Orders{ get; set; }
   public DbSet<OrderItems> OrderItems { get; set; }

 }
}
        