using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;
using my_cosmetic_store.Dtos.Response;


namespace my_cosmetic_store.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Cart_ItemRepository _cart_ItemRepository;

        public CartService(ApiOptions apiOptions, DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _cartRepository = new CartRepository(apiOptions, context, mapper);
            _cart_ItemRepository = new Cart_ItemRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public object GetCart(int UserID)
        {
            var cartFind = _cartRepository.FindByCondition(x => x.UserID == UserID).FirstOrDefault();
            if (cartFind != null)
            {
                cartFind.Cart_Items = _cart_ItemRepository.FindByCondition(x => x.CartID == cartFind.CartID).ToList();
                return new { 
                    CartDetails = cartFind,
                    TotalPrice = cartFind.Cart_Items.Sum(x => x.CartItemQuantity * x.CartItemPrice)
                };
            }
            return null;
        }

        public object CreateCart(CreateNewCarRequest request, int UserID)
        {
            var checkCart = _cartRepository.FindByCondition(x => x.UserID == UserID).FirstOrDefault();
            if (checkCart == null && request.Items.Count > 0)
            {
                Cart newCart = new Cart
                {
                    UserID = UserID,
                    Cart_Items = new List<Cart_Item>()
                };
                _cartRepository.Create(newCart);
                var listCartItemDto = new List<Cart_Item>();
                foreach(var item in request.Items)
                {
                    var cartItem = new Cart_Item
                    {
                        CartID = newCart.CartID,
                        ProductID = item.ProductID,
                        CartItemQuantity = item.Quantity,
                        CartItemPrice = item.Price,
                    };
                    listCartItemDto.Add(cartItem);
                }
                _cart_ItemRepository.AddRangeAsync(listCartItemDto);
                newCart.Cart_Items = listCartItemDto;
                return newCart;
            }
            return checkCart;
        }

        public object DeleteCart(int CartID, int UserID)
        {
            var checkCart = _cartRepository.FindByCondition(x => x.CartID == CartID && x.UserID == UserID).FirstOrDefault();
            if (checkCart == null)
            {
                return null;
            }
            var checkCartItems = _cart_ItemRepository.FindByCondition(x => x.CartID == checkCart.CartID);
            _cart_ItemRepository.DeleteRange(checkCartItems);
            _cartRepository.DeleteByEntity(checkCart);
            return checkCart;
        }
    }
}
