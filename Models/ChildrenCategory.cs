using System.ComponentModel.DataAnnotations;

namespace my_cosmetic_store.Models
{
    public class ChildrenCategory : BaseModel
    {
        [Key]
        public int ChildrenCategoryID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
    }
}
