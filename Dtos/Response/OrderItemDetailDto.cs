using my_cosmetic_store.Models;

namespace my_cosmetic_store.Dtos.Response
{
    public class OrderItemDetailDto
    {
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public decimal? Price { get; set; }
        public decimal SubTotal { get; set; }
        public string Variant { get; set; }
        public decimal PriceOfVariant { get; set; }
    }
}
