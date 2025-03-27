using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Variant
    {
        [Key]
        public int VariantId { get; set; }
        public string VariantName { get; set; }
        public int StockOfVariant { get; set; }
        public decimal PriceOfVariant { get; set; }
    }
}
