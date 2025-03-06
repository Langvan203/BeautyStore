using AutoMapper;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class OrderService
    {
        private readonly OrderRepository orderRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrderService(OrderRepository orderRepository, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            orderRepository = orderRepository;
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
    }
}
