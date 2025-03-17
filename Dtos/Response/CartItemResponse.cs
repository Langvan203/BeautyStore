namespace my_cosmetic_store.Dtos.Response
{
    public class CartItemResponse
    {
        public int UserID { get; set; }

        public List<CartItemDetailResponse> Details { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsCheckOut { get; set; }
    }
}
