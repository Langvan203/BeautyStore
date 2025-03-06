using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Payment : BaseModel
    {
        [Key]
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentStatus { get; set; } = 1;
        public decimal Amount { get; set; }
    }
}
