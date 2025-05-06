using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class Connection
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
