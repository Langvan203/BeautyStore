using AutoMapper;
using my_cosmetic_store.Models;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Repository
{
    public class MessageRepository : BaseRepository<Message>
    {
        private IMapper _mapper;
        public MessageRepository(ApiOptions apiOptions, DatabaseContext context, IMapper mapper) : base(apiOptions, context)
        {
            _mapper = mapper;
        }
    }
}
