using my_cosmetic_store.Models;

namespace my_cosmetic_store.Dtos.Response
{
    public class CartItemDetailResponse
    {
        public int Cart_ItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Variant { get; set; }
        public  double Discount { get; set; }
        public decimal SubTotal { get; set; }

    }
}
