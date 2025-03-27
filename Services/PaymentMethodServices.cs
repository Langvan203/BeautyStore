using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class PaymentMethodServices
    {

        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DatabaseContext _context;
        private readonly PaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodServices(ApiOptions apiOptions, DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _paymentMethodRepository = new PaymentMethodRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public object CreateNewPaymentMethod(CreateNewPaymentMethod request, int UserID)
        {
            var findPaymentMethod = _paymentMethodRepository.FindByCondition(x => x.PaymentType == request.PaymentType && x.UserID == UserID).FirstOrDefault();
            if (findPaymentMethod == null)
            {
                var newPaymentMethod = _mapper.Map<PaymentMethod>(request);
                newPaymentMethod.UserID = UserID;
                _paymentMethodRepository.Create(newPaymentMethod);
                return newPaymentMethod;
            }
            return null;
        }

        public object GetAllPaymentMethod(int UserID)
        {
            var findPayment = _paymentMethodRepository.FindByCondition(x => x.UserID == UserID).ToList();
            return findPayment;
        }

        public object DeletePaymentMethod(int UserID, int PaymentMethodType)
        {
            var findPayment = _paymentMethodRepository.FindByCondition(x => x.UserID == UserID && x.PaymentType == PaymentMethodType).FirstOrDefault();
            if (findPayment == null)
                return null;
            _paymentMethodRepository.DeleteByEntity(findPayment);
            return findPayment;
        }
    }
}
