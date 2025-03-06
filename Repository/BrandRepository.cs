using AutoMapper;
using my_cosmetic_store.Models;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Repository
{
    public class BrandRepository : BaseRepository<Brand>
    {
        private IMapper _mapper;
        public BrandRepository(ApiOptions apiOptions,DatabaseContext context, IMapper mapper) : base(apiOptions,context)
        {
            _mapper = mapper;
        }
    }
}
