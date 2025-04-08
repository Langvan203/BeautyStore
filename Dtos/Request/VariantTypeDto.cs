using System.Text.Json.Serialization;
using System.Text.Json;

namespace my_cosmetic_store.Dtos.Request
{
    public class VariantTypeDto
    {
        public int VariantID { get; set; }
        public decimal VariantPrice { get; set; }
        public int VariantStock { get; set; }
    }
    
}
