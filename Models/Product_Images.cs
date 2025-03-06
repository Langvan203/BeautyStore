using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Product_Images : BaseModel
    {
        [Key]
        public int ImageID { get; set; }
        public string ImageUrl { get; set; }
        public int ProductID { get; set; }
        public int Is_primary { get; set; } = 0;
    }
}
