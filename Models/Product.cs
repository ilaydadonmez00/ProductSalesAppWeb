using ProductSalesApp.Models;
namespace ProductSalesApp.Models
{
    public class Product
    {
        public string Barcode {  get; set; }
        public string Name {  get; set; }
        public int Quantity {  get; set; }
        public decimal UnitPrice { get; set; }

    }
}
