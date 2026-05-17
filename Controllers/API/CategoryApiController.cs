using EcommerceApp.Models;
using ECommerceApp.Data;
using ECommerceApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/categoriesapi
        [HttpGet]
        public ActionResult<IEnumerable<CategoryReadDto>> GetCategories()
        {
            var categories = _context.Categories
                .Include(c => c.Products)
                .AsNoTracking()
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = c.Products.Count
                })
                .ToList();

            return Ok(categories);
        }

        // GET by ID
        [HttpGet("{id}")]
        public ActionResult<CategoryReadDto> GetCategoryById(int id)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = c.Products.Count
                })
                .FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // CREATE
        [HttpPost]
        public ActionResult<CategoryReadDto> CreateCategory(CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            var result = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                ProductCount = 0
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.Id)
            {
                return BadRequest("Route id and body id do not match.");
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = dto.Name;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            // Important protection (same logic style as product validation)
            if (category.Products.Any())
            {
                return BadRequest("Cannot delete category with products.");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }

        // SEARCH
        [HttpGet("search")]
        public ActionResult<IEnumerable<CategoryReadDto>> SearchCategories(string? name)
        {
            var query = _context.Categories
                .Include(c => c.Products)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            var categories = query
                .AsNoTracking()
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = c.Products.Count
                })
                .ToList();

            return Ok(categories);
        }
    }
}