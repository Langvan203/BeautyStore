namespace my_cosmetic_store.Dtos.Request
{
    public class UpdateCategoryRequest
    {
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public IFormFile? thumbNail { get; set; }
    }
}
