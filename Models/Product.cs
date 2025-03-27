using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Product : BaseModel
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? ProductStock { get; set; }
        public decimal? ProductDiscount { get; set; } = 0;
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public string? ProductIngredient { get; set; }
        public string? ProductUserManual { get; set; }
        public ICollection<Product_Images> ProductImages { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
