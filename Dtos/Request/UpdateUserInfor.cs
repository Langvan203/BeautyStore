namespace my_cosmetic_store.Dtos.Request
{
    public class UpdateUserInfor
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public int Gender { get; set; } = 1;
    }
}
