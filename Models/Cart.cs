using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Cart : BaseModel
    {
        [Key]
        public int CartID { get; set; }
        public int UserID { get; set; }
    }
}
