namespace my_cosmetic_store.Dtos.Request
{
    public class CartItemDto
    {
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public int VariantID { get; set; }

        public int ColorID { get; set; }
    }
}
