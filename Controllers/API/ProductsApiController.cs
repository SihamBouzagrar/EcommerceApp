using EcommerceApp.Models;
using ECommerceApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.DTOs;
namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/productsapi
        /*   [HttpGet]
           public ActionResult<IEnumerable<Product>> GetProducts()
           {
               var products = _context.Products
                   .AsNoTracking()
                   .ToList();

               return Ok(products);
           }
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
          [HttpGet("{id}")]
          public IActionResult GetProductById(int id)
          {
              var product = _context.Products
              .AsNoTracking()
              .FirstOrDefault(p => p.Id == id);
              if (product == null)
              {
                  return NotFound(); // 404
              }
              return Ok(product); // 200 + JSON
          }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != product.Id)
            {
                return BadRequest();
            }
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            _context.SaveChanges();
            return Ok(existingProduct);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok(product); // or NoContent()
        }
        [HttpGet("search")]
        public IActionResult SearchProducts(string name)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            var products = query
            .AsNoTracking()
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.StockQuantity,
                p.CategoryId
            })
            .ToList();
            return Ok(products);
        }*/
        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDto>> GetProducts()
        {
            var products = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .ToList();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public ActionResult<ProductReadDto> GetProductById(int id)
        {
            var product = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public ActionResult<ProductReadDto> CreateProduct(ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryExists = _context.Categories.Any(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid CategoryId.");
            }
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            var result = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.Id == product.Id)
            .Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .First();
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
    
public IActionResult UpdateProduct(int id, ProductUpdateDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);

    if (existingProduct == null)
        return NotFound();

    var categoryExists = _context.Categories.Any(c => c.Id == dto.CategoryId);
    if (!categoryExists)
        return BadRequest("Invalid CategoryId.");

    existingProduct.Name = dto.Name;
    existingProduct.Description = dto.Description;
    existingProduct.Price = dto.Price;
    existingProduct.StockQuantity = dto.StockQuantity;
    existingProduct.CategoryId = dto.CategoryId;

    _context.SaveChanges();

    return NoContent();
}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpGet("search")]
        public ActionResult<IEnumerable<ProductReadDto>> SearchProducts(string? name)
        {
            var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p =>
                p.Name.Contains(name) ||
                (p.Description != null && p.Description.Contains(name)));
            }
            var products = query
            .AsNoTracking()
            .Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .ToList();
            return Ok(products);
        }
    }
}