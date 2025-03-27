using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Dtos.Response;
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
        [Authorize]
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
        [Authorize]
        public MessageData AddToCart(CartItemDto request)
        {
            try
            {
                var newCart = _cartService.AddToCart(request, UserIDLogined);
                return new MessageData { Data = newCart, Status = 1 };
            }
            catch (Exception ex) {
                return NG(ex);
            }
        }
        [HttpPut("update")]
        [Authorize]
        public MessageData UpdateCartItem(int cartItemId, int quantity)
        {
            try
            {
                var cart = _cartService.UpdateCartItemAsync(UserIDLogined, cartItemId, quantity);
                return new MessageData { Data = cart, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
            
        }
        [HttpDelete("remove")]
        [Authorize]
        public MessageData RemoveCartItem(int CartItemID) {
            try
            {
                var cartDelete = _cartService.RemoveCartItemAsync(CartItemID, UserIDLogined);
                return new MessageData { Data = cartDelete, Des = "Delete cart success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpDelete("DeleteCart")]
        [Authorize]
        public MessageData DeleteCart()
        {
            try
            {
                var cartDelete = _cartService.RemoveCart(UserIDLogined);
                return new MessageData { Data = cartDelete, Des = "Delete cart success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpPost("checkout")]
        [Authorize]
        public async Task<MessageData> Checkout(CheckOutDto dto)
        {
            try
            {
                var cartDelete = await _cartService.CheckoutAsync(UserIDLogined, dto);
                return new MessageData { Data = cartDelete, Des = "Delete cart success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
