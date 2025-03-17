using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Dtos.Request
{
    public class CreateImageDto
    {
        [DataType(DataType.Upload)]
        [Required]
        public IFormFile File { get; set; }
        public bool IsMain { get; set; }
    }
}
