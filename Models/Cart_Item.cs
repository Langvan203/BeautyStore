using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Cart_Item : BaseModel
    {
        [Key]
        public int Cart_ItemID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int CartItemQuantity { get; set; }

        public decimal CartItemPrice { get; set; }
    }
}
