using ProductSalesApp.Models;

namespace ProductSalesApp.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Product.UnitPrice * Quantity;


    }
}

