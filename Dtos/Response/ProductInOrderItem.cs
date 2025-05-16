namespace my_cosmetic_store.Dtos.Response
{
    public class ProductInOrderItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal? discount { get; set; }
        public decimal? finalPrice { get; set; }
        public int quantity { get; set; }   
        public string image { get; set; }
        public string variant { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
    }
}
