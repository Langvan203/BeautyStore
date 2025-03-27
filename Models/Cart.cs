using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_cosmetic_store.Models
{
    public class Cart : BaseModel
    {
        [Key]
        public int CartID { get; set; }
        public int UserID { get; set; }
        public bool IsCheckOut { get; set; } = false;
        public ICollection<Cart_Item> Cart_Items { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
