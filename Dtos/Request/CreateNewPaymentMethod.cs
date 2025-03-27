namespace my_cosmetic_store.Dtos.Request
{
    public class CreateNewPaymentMethod
    {
        public int PaymentType { get; set; } // 1: Visa // 2: mastercard: // 3: JCB // 4: American Expresss
        public string PaymentName { get; set; }
        public string CVV { get; set; }
        public string CardNumber { get; set; }
        public DateTime? OuterDateUsing { get; set; } = DateTime.UtcNow;
    }
}
