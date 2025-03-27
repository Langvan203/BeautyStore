namespace my_cosmetic_store.Dtos.Request
{
    public class UpdateProductRequest
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int? ProductStock { get; set; }
        public decimal? ProductDiscount { get; set; }
        public string? ProductIngredient { get; set; }
        public string? ProductUserManual { get; set; }
        public List<IFormFile>? Files { get; set; }
        public int MainImageIndex { get; set; }
        public string VariantID { get; set; }
        public string ExistingImageIdsToKeep { get; set; }

    }
}
