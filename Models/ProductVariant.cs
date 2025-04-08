using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }
        public int ProductID { get; set; }
        public int StockOfVariant { get; set; }
        public decimal PriceOfVariant { get; set; }
        public Product Product { get; set; }
        public int VariantId { get; set; }
        public Variant Variant { get; set; }
    }
}
