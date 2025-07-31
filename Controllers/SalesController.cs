using Microsoft.AspNetCore.Mvc;
using ProductSalesApp.Models;
using ProductSalesAppWeb.Models;
using System.Linq;

namespace ProductSalesApp.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;
        private static Cart cart = new();

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Products()
        {
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(string barcode, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Barcode == barcode);

            if (product != null)
            {
                if (product.Quantity >= quantity)
                {
                    product.Quantity -= quantity;
                    _context.SaveChanges();

                    cart.AddItem(product, quantity);

                    return Json(new
                    {
                        success = true,
                        message = $"{quantity} x {product.Name} added to cart."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Not enough stock for {product.Name}. Available: {product.Quantity}"
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Product not found!"
            });
        }


        [HttpPost]
        public IActionResult CompleteSale()
        {
            cart = new Cart();
            return RedirectToAction("OrderComplete");
        }


        [HttpPost]
        public IActionResult CancelSale()
        {
            foreach (var item in cart.Items)
            {
                var productInDb = _context.Products.FirstOrDefault(p => p.Barcode == item.Product.Barcode);
                if (productInDb != null)
                {
                    productInDb.Quantity += item.Quantity;
                }
            }

            _context.SaveChanges();
            cart = new Cart();
            return RedirectToAction("OrderCanceled");
        }

        public IActionResult OrderComplete()
        {
            return View();
        }

        public PartialViewResult GetCartPartial()
        {
            ViewBag.Cart = cart;
            return PartialView("_CartPartial");
        }

        public IActionResult Home()
        {
            ViewBag.Cart = cart;
            return View();
        }

        public PartialViewResult GetCartSummary()
        {
            ViewBag.Cart = cart;
            return PartialView("_CartSummary");
        }

        public IActionResult ReviewCart()
        {
            ViewBag.Cart = cart;
            return View();
        }

        
        public IActionResult OrderCanceled()
        {
            return View();
        }

        public IActionResult GetProductList()
        {
            var products = _context.Products.ToList();
            return PartialView("_ProductListPartial", products); 
        }


        [HttpPost]
        public IActionResult RemoveFromCart(string barcode, int quantity)
        {
            var cartItem = cart.Items.FirstOrDefault(i => i.Product.Barcode == barcode);

            if (cartItem != null)
            {
                
                var productInDb = _context.Products.FirstOrDefault(p => p.Barcode == barcode);

                if (productInDb != null)
                {
                    
                    if (quantity >= cartItem.Quantity)
                    {
                        productInDb.Quantity += cartItem.Quantity; 
                        cart.Items.Remove(cartItem);
                    }
                    else
                    {
                        productInDb.Quantity += quantity; 
                        cartItem.Quantity -= quantity;
                    }

                    _context.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        message = $"{quantity} x {cartItem.Product.Name} removed from cart."
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Product not found in cart."
            });
        }

    }
}

