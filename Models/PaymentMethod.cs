namespace my_cosmetic_store.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public int PaymentType { get; set; } // 1: Visa // 2: mastercard: // 3: JCB // 4: American Expresss
        public string PaymentName { get; set; }
        public int UserID { get; set; }
        public string CardNumber { get; set; }
        public DateTime OuterDateUsing { get; set; } = DateTime.Now;
        public string CVV { get; set; }
        public User User { get; set; }
    }
}
