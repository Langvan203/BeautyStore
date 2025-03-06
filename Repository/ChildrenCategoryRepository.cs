using AutoMapper;
using my_cosmetic_store.Models;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Repository
{
    public class ChildrenCategoryRepository : BaseRepository<ChildrenCategory>
    {
        private IMapper _mapper;
        public ChildrenCategoryRepository(ApiOptions apiOptions, DatabaseContext context, IMapper mapper) : base(apiOptions, context)
        {
            _mapper = mapper;
        }
    }
   
}
