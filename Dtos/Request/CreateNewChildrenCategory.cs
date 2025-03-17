namespace my_cosmetic_store.Dtos.Request
{
    public class CreateNewChildrenCategory
    {
        public string Name { get; set; }
        public int ParentID { get; set; }
    }
}
