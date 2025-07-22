using Microsoft.AspNetCore.Mvc;
using ProductSalesApp.Models;
using ProductSalesAppWeb.Models;

namespace ProductSalesApp.Controllers
{
    public class SalesController : Controller
    {
        private static List<Product> products = new()
        {
            new Product { Barcode = "4001", Name = "Milk (1L)", Quantity = 100, UnitPrice = 25.00m },
            new Product { Barcode = "4002", Name = "Eggs (10 pcs)", Quantity = 80, UnitPrice = 45.00m },
            new Product { Barcode = "4003", Name = "Sugar (1kg)", Quantity = 60, UnitPrice = 50.00m },
            new Product { Barcode = "4004", Name = "Sunflower Oil (1L)", Quantity = 40, UnitPrice = 95.00m },
            new Product { Barcode = "4005", Name = "Pasta (500g)", Quantity = 150, UnitPrice = 20.00m },
            new Product { Barcode = "4006", Name = "Rice (1kg)", Quantity = 90, UnitPrice = 60.00m },
            new Product { Barcode = "4007", Name = "Tea (500g)", Quantity = 70, UnitPrice = 85.00m },
            new Product { Barcode = "4008", Name = "Coffee (100g)", Quantity = 50, UnitPrice = 120.00m },
            new Product { Barcode = "4009", Name = "Biscuits (pack)", Quantity = 200, UnitPrice = 15.00m },
            new Product { Barcode = "4010", Name = "Cheese (250g)", Quantity = 60, UnitPrice = 80.00m },
            new Product { Barcode = "4011", Name = "Apple (kg)", Quantity = 100, UnitPrice = 35.00m },
            new Product { Barcode = "4012", Name = "Banana (kg)", Quantity = 80, UnitPrice = 40.00m },
            new Product { Barcode = "4013", Name = "Tomato (kg)", Quantity = 120, UnitPrice = 25.00m }
        };

        private static Cart cart = new();

        public IActionResult Index()
        {
            ViewBag.Products = products;
            ViewBag.Cart = cart;

            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(string barcode, int quantity)
        {
            var product = products.FirstOrDefault(p => p.Barcode == barcode);

            if (product != null)
            {
                if (product.Quantity >= quantity)
                {
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
            cart = new Cart(); // sepeti sıfırla
            return RedirectToAction("OrderComplete");
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
        [HttpPost]
        public IActionResult CancelSale()
        {
            // Sepetteki ürünleri stoğa geri ekle
            foreach (var item in cart.Items)
            {
                var product = products.FirstOrDefault(p => p.Barcode == item.Product.Barcode);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }

            cart = new Cart(); // sepeti sıfırla
            return RedirectToAction("OrderCanceled");
        }


        public IActionResult OrderCanceled()
        {
            return View();
        }

        public IActionResult GetProductList()
        {
            ViewBag.Products = products;
            return PartialView("_ProductListPartial");
        }
        [HttpPost]
        public IActionResult RemoveFromCart(string barcode, int quantity)
        {
            var cartItem = cart.Items.FirstOrDefault(i => i.Product.Barcode == barcode);
            if (cartItem != null)
            {
                if (quantity >= cartItem.Quantity)
                {
                    // Sepetten tamamen çıkar
                    cart.Items.Remove(cartItem);
                    cartItem.Product.Quantity += cartItem.Quantity;
                }
                else
                {
                    // Sadece belirlenen miktarı düş
                    cartItem.Quantity -= quantity;
                    cartItem.Product.Quantity += quantity;
                }

                return Json(new { success = true, message = $"{quantity} x {cartItem.Product.Name} removed from cart." });
            }

            return Json(new { success = false, message = "Product not found in cart." });
        }


    }
}
