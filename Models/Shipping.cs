using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Shipping : BaseModel
    {
        [Key]
        public int ShippingID { get; set; }
        public string ShippingCompany { get; set; }
        public string Tracking_number { get; set; }
        public string ShippingStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int OrderID { get; set; }
    }
}
