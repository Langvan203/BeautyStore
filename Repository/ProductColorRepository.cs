using AutoMapper;
using my_cosmetic_store.Models;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Repository
{
    public class ProductColorRepository : BaseRepository<ProductColor>
    {
        private IMapper _mapper;
        public ProductColorRepository(ApiOptions apiOptions, DatabaseContext context, IMapper mapper) : base(apiOptions, context)
        {
            _mapper = mapper;
        }
    }
    
}
