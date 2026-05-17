using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;
namespace EcommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId, string? search)
        {
            // Load categories for dropdown
            ViewBag.Categories = new SelectList(
                _context.Categories
                    .AsNoTracking()
                    .ToList(),
                "Id",
                "Name",
                categoryId
            );

            ViewBag.CurrentCategoryId = categoryId;

            // Build query
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .AsQueryable();

            // Apply category filter
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(p =>
                p.Name.Contains(search) ||
(p.Category != null && p.Category.Name.Contains(search))
);
            }
            // Execute query (VERY IMPORTANT: ToList at the end
            var products = query.ToList();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategoryId = categoryId;
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Product";
            // NEW: categories dropdown data
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View(); // empty form
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            // If validation fails, we must repopulate dropdown before returning View()
            // Server-side validation (Data Annotations)
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name",
product.CategoryId);

                return View(product); // return form + errors
            }
            // Ensure CreatedAt has a value (optional safety)
            if (product.CreatedAt == default)
                product.CreatedAt = DateTime.Now;
            // NEW (recommended): validate CategoryId exists (server-side safety)
            bool categoryExists = _context.Categories.Any(c => c.Id == product.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Please select a valid category.");
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name",
                product.CategoryId);
                return View(product);
            }
            // Add and save
            _context.Products.Add(product);
            _context.SaveChanges();
            // Redirect to Index after success (PRG pattern)
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            // Retrieve product by id
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            // If not found, return 404
            if (product == null)
                return NotFound();
            // Pass product to view
            return View(product);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve product by id
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            // If not found, return 404
            if (product == null)
                return NotFound();
            // Optional: Set page title
            ViewData["Title"] = "Edit Product";

            // NEW: categories dropdown data (with selected value)
            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                "Id",
                "Name",
                product.CategoryId   // 👈 pre-select current category
            );
            // Send product to view (pre-filled form)
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            // Safety check: route id must match model id
            if (id != product.Id)
                return BadRequest();
            // Validate model (Data Annotations)
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    _context.Categories.ToList(),
                    "Id",
                    "Name",
                    product.CategoryId
                );
                return View(product);
            }
            // Retrieve existing product from DB
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
                return NotFound();
            // Server-side safety: check category exists
            bool categoryExists = _context.Categories.Any(c => c.Id == product.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Please select a valid category.");

                ViewBag.Categories = new SelectList(
                    _context.Categories.ToList(),
                    "Id",
                    "Name",
                    product.CategoryId
                );

                return View(product);
            }

            // Update fields
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.CreatedAt = product.CreatedAt;
            // optional: you may keep original
            // Save changes
            _context.SaveChanges();
            // Redirect to list
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve product by id (to display confirmation)
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id, Product product)

        {

            if (product == null)
                return NotFound();
            // Remove product
            _context.Products.Remove(product);
            // Save changes
            _context.SaveChanges();
            // Redirect to list
            return RedirectToAction(nameof(Index));
        }


    }
}