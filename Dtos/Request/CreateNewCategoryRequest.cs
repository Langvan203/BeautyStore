namespace my_cosmetic_store.Dtos.Request
{
    public class CreateNewCategoryRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public IFormFile? ThumbNail { get; set; }
    }
}
