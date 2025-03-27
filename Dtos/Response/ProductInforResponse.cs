using my_cosmetic_store.Models;

namespace my_cosmetic_store.Dtos.Response
{
    public class ProductInforResponse
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? ProductStock { get; set; }
        public decimal? ProductDiscount { get; set; }
        public ICollection<ProductImageResponse> ProductImages { get; set; }
    }
}
