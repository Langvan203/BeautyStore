using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class User : BaseModel
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public int Role { get; set; } = 0;
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; } = 1;
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
