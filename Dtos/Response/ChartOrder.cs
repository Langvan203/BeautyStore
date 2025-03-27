namespace my_cosmetic_store.Dtos.Response
{
    public class ChartOrder
    {
        public int totalOrder { get; set; }
        public int totalProduct { get; set; }

        public decimal totalRevenue { get; set; }

        public int totalUsers { get; set; }
    }
}
