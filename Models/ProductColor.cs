using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class ProductColor
    {
        [Key]
        public int Id { get; set; }
        public int ProductID { get; set; }
        public int? StockOfColor { get; set; }
        public Product Product { get; set; }
        public int ColorID { get; set; }
        public Color Color { get; set; }
    }
}
