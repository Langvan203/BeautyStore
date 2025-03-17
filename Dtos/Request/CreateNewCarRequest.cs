namespace my_cosmetic_store.Dtos.Request
{
    public class CreateNewCarRequest
    {
        public int UserID { get; set; }

        public ICollection<AddCartItemRequest> Items { get; set; }
    }
}
