using AutoMapper;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class Order_ItemService
    {
        private readonly Order_ItemRepository _order_ItemRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Order_ItemService(Order_ItemRepository order_ItemRepository, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _order_ItemRepository = order_ItemRepository;
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
    }
}
