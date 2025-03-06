using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Brand : BaseModel
    {
        [Key]
        public int BrandID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
