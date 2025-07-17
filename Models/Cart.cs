using ProductSalesApp.Models;
namespace ProductSalesApp.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.TotalPrice);

        public void AddItem(Product product, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.Product.Barcode == product.Barcode);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Product = product, Quantity = quantity });
            }

            product.Quantity -= quantity;

        }
    }
}
