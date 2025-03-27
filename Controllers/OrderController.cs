using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_cosmetic_store.Models;
using my_cosmetic_store.Services;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController<OrderController>
    {
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(DatabaseContext context, ApiOptions options, IMapper mapper, IWebHostEnvironment webhost)
        {
            _mapper = mapper;
            _orderService = new OrderService(options, mapper, context, webhost);
        }

        [HttpGet("GetOrder")]
        [Authorize]
        public MessageData GetOrder()
        {
            try
            {
                var order = _orderService.GetOrdersByUserIdAsync(UserIDLogined);
                return new MessageData { Data = order, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpGet("GetAllOrderAdmin")]
        [Authorize(Roles = "1")]
        public MessageData GetAllOrderAdmin()
        {
            try
            {
                var order = _orderService.GetAllOrderAdmin();
                return new MessageData { Data = order, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet("GetOrderID-details")]
        [Authorize]
        public MessageData GetOrderById(int Id)
        {
            try
            {
                var order = _orderService.GetOrderByIdAsync(Id, UserIDLogined);
                return new MessageData { Data = order, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet("Get-order-chart")]
        [Authorize(Roles = "1")]
        public MessageData GetChartOrder(int year)
        {
            try
            {
                var orders = _orderService.GetChartOrder(year);
                return new MessageData { Data = orders, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }

        }
    }
}
