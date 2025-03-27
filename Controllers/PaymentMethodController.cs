using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Services;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : BaseApiController<PaymentMethodController>
    {
        private readonly PaymentMethodServices paymentService;
        private readonly IMapper _mapper;
        public PaymentMethodController(DatabaseContext context, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHost)
        {
            _mapper = mapper;
            paymentService = new PaymentMethodServices(apiOptions, context, mapper, webHost);
        }

        [HttpGet("GetAllPaymentMethod")]
        [Authorize]
        public MessageData GetAllPaymentMethod()
        {
            try
            {
                var paymentmethod = paymentService.GetAllPaymentMethod(UserIDLogined);
                return new MessageData { Data = paymentmethod, Status = 1 };
            }
            catch (Exception ex) 
            {
                return NG(ex);
            }
        }

        [HttpPost("Create-new-paymentmethod")]
        [Authorize]
        public MessageData CreateNewPaymentMethod(CreateNewPaymentMethod request)
        {
            try
            {
                var paymentmethod = paymentService.CreateNewPaymentMethod(request, UserIDLogined);
                return new MessageData { Data = paymentmethod, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpDelete("Delete-paymentmethod")]
        [Authorize]
        public MessageData DeletePaymentMethod(int PaymentMethodType)
        {
            try
            {
                var paymentmethod = paymentService.DeletePaymentMethod(UserIDLogined, PaymentMethodType);
                return new MessageData { Data = paymentmethod, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
