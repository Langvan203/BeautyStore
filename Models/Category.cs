using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Category : BaseModel
    {
        [Key]
        public int CategoryID { get; set; } 
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? thumbNail { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
