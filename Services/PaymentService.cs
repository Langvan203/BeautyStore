using AutoMapper;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly DatabaseContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PaymentService(ApiOptions apiOptions, IMapper mapper,DatabaseContext _context ,IWebHostEnvironment webHostEnvironment)
        {
            _paymentRepository = new PaymentRepository(apiOptions, _context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            context = _context;
        }
    }
}
