using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Order_Item : BaseModel
    {
        [Key]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  
    }
}
