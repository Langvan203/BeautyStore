namespace my_cosmetic_store.Utility
{
    public class ApiOptions
    {
        public string StringConnection { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public string BaseUrl { get; set; }
    }
}
