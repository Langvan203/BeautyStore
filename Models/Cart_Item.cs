using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_cosmetic_store.Models
{
    public class Cart_Item : BaseModel
    {
        [Key]
        public int Cart_ItemID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int? VariantID { get; set; }
        public int CartItemQuantity { get; set; }
        public decimal? CartItemPrice { get; set; }
        [ForeignKey("CartID")]        
        public Cart Cart { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
