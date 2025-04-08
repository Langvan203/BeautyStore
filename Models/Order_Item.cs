using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_cosmetic_store.Models
{
    public class Order_Item : BaseModel
    {
        [Key]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int? ProductVariantId { get; set; }
        public decimal? Price { get; set; }
        public Order Order { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public decimal PriceOfVariant { get; set; }
        public ProductVariant ProductVariant { get; set; }

    }
}
