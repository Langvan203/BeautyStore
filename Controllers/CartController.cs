using AutoMapper;
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
    public class CartController : BaseApiController<CartController>
    {
        private readonly CartService _cartService;
        private readonly IMapper _mapper;
        public CartController(DatabaseContext context, ApiOptions options, IMapper mapper, IWebHostEnvironment webhost)
        {
            _mapper = mapper;
            _cartService = new CartService(options, context, mapper, webhost);
        }

        [HttpGet("GetYourCart")]
        public MessageData GetCart()
        {
            try
            {
                var cartList = _cartService.GetCart(UserIDLogined);
                return new MessageData { Data = cartList, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpPost("CreateNewCart")]
        public MessageData CreateCart(CreateNewCarRequest request)
        {
            try
            {
                var newCart = _cartService.CreateCart(request, UserIDLogined);
                return new MessageData { Data = newCart, Status = 1 };
            }
            catch (Exception ex) {
                return NG(ex);
            }
        }
        [HttpPost("DeleCartAdmin")]
        public MessageData DelteCart(int CartID) {
            try
            {
                var cartDelete = _cartService.DeleteCart(CartID, UserIDLogined);
                return new MessageData { Data = cartDelete, Des = "Delete cart success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
