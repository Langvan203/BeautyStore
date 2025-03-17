namespace my_cosmetic_store.Dtos.Request
{
    public class AddCartItemRequest
    {
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
