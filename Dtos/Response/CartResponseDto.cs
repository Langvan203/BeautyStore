namespace my_cosmetic_store.Dtos.Response
{
    public class CartResponseDto
    {
        public int CartID { get; set; }

        public int UserID { get; set; }
        public List<CartItemDetailResponse> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
