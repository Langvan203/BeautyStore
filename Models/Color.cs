using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Color
    {
        [Key]
        public int ColorID { get; set; }
        public string ColorHexaValue { get; set; }
        public string ColorName { get; set; }
        public int? StockOfColor { get; set; }
    }
}
