using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;

using ECommerceApp.ViewModels;
using EcommerceApp.Models;


namespace ECommerceApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Orders/Checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            ViewBag.Products = new SelectList(
                _context.Products.AsNoTracking().ToList(),
                "Id",
                "Name"
            );
            return View();
        }

        // POST: /Orders/Checkout
        [HttpPost]
        public IActionResult Checkout(CreateOrderViewModel model)
        {
            ViewBag.Products = new SelectList(
                _context.Products.AsNoTracking().ToList(),
                "Id",
                "Name",
                model.ProductId
            );

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using var transaction = _context.Database.BeginTransaction();

            var product = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);

            if (product == null)
            {
                ModelState.AddModelError("", "Selected product was not found.");
                return View(model);
            }

            if (product.StockQuantity - model.Quantity < 0)
            {
                throw new Exception("Insufficient stock. Transaction rolled back.");
            }

            var order = new Order
            {
                CreatedAt = DateTime.Now,
                TotalAmount = product.Price * model.Quantity
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            var orderItem = new OrderItems
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = model.Quantity,
                UnitPrice = product.Price
            };

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();

            transaction.Commit();

            TempData["Success"] = "Order created successfully.";

            return RedirectToAction("Index", "Products");
        }
    }
}
