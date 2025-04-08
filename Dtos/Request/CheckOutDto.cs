namespace my_cosmetic_store.Dtos.Request
{
    public class CheckOutDto
    {
        public string ShippingAdress { get; set; }
        public string PhoneNumber { get; set; }
        public string ReceiverName { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; }
    }
}
