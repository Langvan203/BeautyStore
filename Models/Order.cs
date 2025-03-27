using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string PhoneNumber { get; set; }
        public string ReceiverName { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public ICollection<Order_Item> Order_Items { get; set; }
        public ICollection<HistoryOder> HistoryOder { get; set; }
    }
}
