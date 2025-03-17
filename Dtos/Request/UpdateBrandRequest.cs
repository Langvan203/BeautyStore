namespace my_cosmetic_store.Dtos.Request
{
    public class UpdateBrandRequest
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? thumbNail { get; set; }
    }
}
