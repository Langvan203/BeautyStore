namespace my_cosmetic_store.Models
{
    public class HistoryOder
    {
        public int Id { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? Title { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}
