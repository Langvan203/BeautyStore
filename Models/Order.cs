using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Order : BaseModel
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total_Amount { get; set; }
        public int Status { get; set; } = 1;
        public string? ShippingAdress { get; set; }
        public int UserID { get; set; }
    }
}
