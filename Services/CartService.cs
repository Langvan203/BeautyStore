using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;
using my_cosmetic_store.Dtos.Response;
using Microsoft.EntityFrameworkCore;
using Azure;


namespace my_cosmetic_store.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly ProductRepository _productRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Cart_ItemRepository _cart_ItemRepository;
        private readonly VariantRepository _variantRepository;
        private readonly ProductVariantRepository _productVariantRepository;
        private readonly HistoryOrderRepository _historyOrderRepository;
        private readonly DatabaseContext _context;

        public CartService(ApiOptions apiOptions, DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _cartRepository = new CartRepository(apiOptions, context, mapper);
            _cart_ItemRepository = new Cart_ItemRepository(apiOptions, context, mapper);
            _productRepository = new ProductRepository(apiOptions, context, mapper);    
            _variantRepository = new VariantRepository(apiOptions, context, mapper);
            _productVariantRepository = new ProductVariantRepository(apiOptions, context, mapper);
            _historyOrderRepository = new HistoryOrderRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public object GetCart(int UserID)
        {
            var cartFind = _cartRepository.FindByCondition(x => x.UserID == UserID && !x.IsCheckOut)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Variant)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductColor)
                    .ThenInclude(x => x.Color)
                .FirstOrDefault();
            if (cartFind == null)
            {
                Cart cart = new Cart()
                {
                    UserID = UserID,
                    Cart_Items = new List<Cart_Item>()
                };
                _cartRepository.Create(cart);
            }
            return MapCartToCartResponseDto(cartFind);
        }
        

        public object AddToCart(CartItemDto request, int UserID)
        {
            var cart = _cartRepository.FindByCondition(x => x.UserID == UserID && !x.IsCheckOut)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductVariant)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductColor)
                    .FirstOrDefault();
            if(cart == null)
            {
                cart = new Cart
                {
                    UserID = UserID,
                    Cart_Items = new List<Cart_Item>()
                };
                _cartRepository.Create(cart);
            }
            var existingItem = cart.Cart_Items.FirstOrDefault(x => x.ProductID == request.ProductID && x.ProductVariantId == request.VariantID && x.ProductColorId == request.ColorID);
            var productDiscount = _productRepository.FindByCondition(x => x.ProductID == request.ProductID).FirstOrDefault().ProductDiscount;
            if (existingItem != null)
            {
                if (existingItem.ProductVariantId == request.VariantID && existingItem.ProductColorId == request.ColorID)
                {
                    existingItem.CartItemPrice += request.Quantity * (existingItem.ProductVariant.PriceOfVariant - (existingItem.ProductVariant.PriceOfVariant * (decimal)productDiscount / 100));
                    existingItem.CartItemQuantity += request.Quantity;
                    _cart_ItemRepository.UpdateByEntity(existingItem);
                }
            }
            else
            {
                var findPriceForVariant = _productVariantRepository.FindByCondition(x => x.VariantId == request.VariantID && x.ProductID == request.ProductID).FirstOrDefault();
                if (findPriceForVariant != null)
                {
                    var newCartItem = new Cart_Item
                    {
                        CartID = cart.CartID,
                        ProductID = request.ProductID,
                        CartItemQuantity = request.Quantity,
                        CartItemPrice = request.Quantity * (findPriceForVariant.PriceOfVariant - (findPriceForVariant.PriceOfVariant * (decimal)productDiscount / 100)),
                        ProductVariantId = request.VariantID,
                        ProductColorId = request.ColorID,
                    };
                    _cart_ItemRepository.Create(newCartItem);
                    //_cartRepository.UpdateByEntity(cart);
                }
                else
                {
                    throw new Exception("Khong tim thay gia tri loai san pham");
                }
            }

            _cartRepository.SaveChange();

            cart = _cartRepository.FindByCondition(x => x.CartID == cart.CartID)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Variant)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductColor)
                    .ThenInclude(x => x.Color)
                .FirstOrDefault();
            return MapCartToCartResponseDto(cart);
        }

        public object UpdateCartItemAsync(int userId, int cartItemId, int quantity)
        {
            var cart = _cartRepository.FindByCondition(x => x.UserID == userId && !x.IsCheckOut)
                .Include(x => x.Cart_Items)
                    .FirstOrDefault();

            if (cart == null)
                throw new Exception("Không tìm thấy giỏ hàng");

            // Tìm item cần cập nhật
            var cartItem = cart.Cart_Items.FirstOrDefault(x => x.Cart_ItemID == cartItemId);
            if (cartItem == null)
                throw new Exception("Không tìm thấy sản phẩm trong giỏ hàng");

            if (quantity <= 0)
            {
                // Nếu số lượng <= 0, xóa khỏi giỏ hàng
                _cart_ItemRepository.DeleteByEntity(cartItem);
            }
            else
            {
                // Cập nhật số lượng
                cartItem.CartItemQuantity = quantity;
            }

            _cartRepository.UpdateByEntity(cart);

            // Tải lại giỏ hàng
            cart = _cartRepository.FindByCondition(x => x.CartID == cart.CartID)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Variant)
                .FirstOrDefault(c => c.CartID == cart.CartID);

            return MapCartToCartResponseDto(cart);
        }

        public object RemoveCartItemAsync(int cartItemId, int userId)
        {
            // Tìm giỏ hàng của người dùng
            var cart = _cartRepository.FindByCondition(x => x.UserID == userId && !x.IsCheckOut)
                .Include(x => x.Cart_Items)
                .FirstOrDefault();

            if (cart == null)
                throw new Exception("Không tìm thấy giỏ hàng");

            // Tìm item cần xóa
            var cartItem = cart.Cart_Items.FirstOrDefault(ci => ci.Cart_ItemID == cartItemId);
            if (cartItem == null)
                throw new Exception("Không tìm thấy sản phẩm trong giỏ hàng");

            _cart_ItemRepository.DeleteByEntity(cartItem);

            // Tải lại giỏ hàng
            cart = _cartRepository.FindByCondition(x => x.CartID == cart.CartID)
               .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Cart_Items)
                    .ThenInclude(x => x.ProductVariant)
                    .ThenInclude(x => x.Variant)
                .FirstOrDefault();

            return MapCartToCartResponseDto(cart);
        }

        public object RemoveCart(int UserID)
        {
            var cartFind = _cartRepository.FindByCondition(x => x.UserID == UserID).FirstOrDefault();
            _cartRepository.DeleteByEntity(cartFind);
            return cartFind;
        }

        public async Task<OrderResponseDto> CheckoutAsync(int userId, CheckOutDto checkoutDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Tìm giỏ hàng hiện tại
                var cart = _cartRepository.FindByCondition(x => x.UserID == userId && !x.IsCheckOut)
                    .Include(x => x.Cart_Items)
                        .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.Cart_Items)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.Cart_Items)
                        .ThenInclude(x => x.ProductColor)
                        .ThenInclude(x => x.Color)
                    .FirstOrDefault();

                if (cart == null || !cart.Cart_Items.Any())
                    throw new Exception("Giỏ hàng trống");


                // Tính tổng tiền
                decimal totalAmount = 0;
                foreach (var item in cart.Cart_Items)
                {
                    var productDiscount = item.ProductVariant.Product.ProductDiscount;
                    var productVariant = item.ProductVariant;
                    var subTotal = productVariant.PriceOfVariant * item.CartItemQuantity - (productVariant.PriceOfVariant * item.CartItemQuantity * (decimal)productDiscount / 100);
                    totalAmount += subTotal;
                }

                // Tạo đơn hàng mới
                var order = new Order
                {
                    UserID = userId,
                    OrderDate = DateTime.Now,
                    Total_Amount = (int)totalAmount,
                    PhoneNumber = checkoutDto.PhoneNumber,
                    ReceiverName = checkoutDto.ReceiverName,
                    Status = 1, // Đang chờ xử lý
                    ShippingAdress = checkoutDto.ShippingAdress,
                    Order_Items = new List<Order_Item>(),
                    PaymentMethod = checkoutDto.PaymentMethod,
                    ShippingMethod = checkoutDto.ShippingMethod,
                };

                // Thêm các sản phẩm vào đơn hàng
                foreach (var item in cart.Cart_Items)
                {
                    // new
                    var orderItem = new Order_Item
                    {
                        ProductID = item.ProductID,
                        Quantity = item.CartItemQuantity,
                        Price = item.ProductVariant.PriceOfVariant - (item.ProductVariant.PriceOfVariant * item.Product.ProductDiscount / 100),
                        ProductVariantId = item.ProductVariant.VariantId,
                        PriceOfVariant = item.ProductVariant.PriceOfVariant,
                        ProductColorId = item.ProductColorId,
                    };
                    order.Order_Items.Add(orderItem);
                }

                _context.Orders.Add(order);

                // Đánh dấu giỏ hàng đã checkout
                cart.IsCheckOut = true;
                _cartRepository.UpdateByEntity(cart);

                HistoryOder historyOder = new HistoryOder()
                {
                    OrderID = order.OrderID,
                    Title = GetOrderStatusString(order.Status),
                    UpdatedDate = DateTime.UtcNow,
                };
                _historyOrderRepository.Create(historyOder);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Tải đơn hàng với chi tiết sản phẩm
                var completeOrder = await _context.Orders
                    .Include(o => o.Order_Items)
                        .ThenInclude(oi => oi.Product)
                        .ThenInclude(oi => oi.ProductImages)
                    .Include(x => x.Order_Items)
                        .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Variant)
                    .Include(x => x.Order_Items)
                        .ThenInclude(x => x.ProductColor)
                        .ThenInclude(x => x.Color)
                    .FirstOrDefaultAsync(o => o.OrderID == order.OrderID);

                return MapOrderToOrderResponseDto(completeOrder);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private CartResponseDto MapCartToCartResponseDto(Cart cart)
        {
            if (cart == null) return null;

            var response = new CartResponseDto
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                Items = new List<CartItemDetailResponse>(),
                TotalPrice = 0
            };

            if (cart.Cart_Items != null)
            {
                foreach (var item in cart.Cart_Items)
                {
                    var product = item.Product;
                    var mainImage = product.ProductImages.Where(x => x.Is_primary == 1).Select(x => x.ImageUrl).FirstOrDefault();
                    var productVariant = item.ProductVariant;
                    var productColor = item.ProductColor;
                    var subTotal = productVariant.PriceOfVariant * item.CartItemQuantity - (productVariant.PriceOfVariant * item.CartItemQuantity * (decimal)product.ProductDiscount / 100);
                    response.Items.Add(new CartItemDetailResponse
                    {
                        Cart_ItemID = item.Cart_ItemID,
                        ProductID = item.ProductID,
                        ProductName = product?.ProductName, // Giả sử Product có trường Name
                        ProductImage = mainImage, // Giả sử Product có trường ImageUrl
                        Quantity = item.CartItemQuantity,
                        Discount = Convert.ToDouble(product.ProductDiscount),
                        Price = productVariant.PriceOfVariant,
                        Variant = productVariant.Variant.VariantName,
                        SubTotal = Convert.ToDecimal(subTotal),
                        ColorName = productColor.Color.ColorName,
                        ColorCode = productColor.Color.ColorHexaValue,
                    });
                    response.TotalPrice += Convert.ToDecimal(subTotal);
                }
            }

            return response;
        }

        private OrderResponseDto MapOrderToOrderResponseDto(Order order)
        {
            if (order == null) return null;

            var response = new OrderResponseDto
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                TotalAmount = order.Total_Amount,
                Status = GetOrderStatusString(order.Status),
                ShippingAddress = order.ShippingAdress,
                Items = new List<OrderItemDetailDto>()
            };

            if (order.Order_Items != null)
            {
                foreach (var item in order.Order_Items)
                {
                    var product = item.Product;
                    var mainImage = product.ProductImages.Where(x => x.Is_primary == 1).Select(x => x.ImageUrl).FirstOrDefault();
                    var productVariant = item.ProductVariant;
                    var productColor = item.ProductColor;
                    response.Items.Add(new OrderItemDetailDto
                    {
                        OrderItemID = item.OrderItemID,
                        ProductID = item.ProductID,
                        ProductName = product?.ProductName, // Giả sử Product có trường Name
                        ProductImage = mainImage, // Giả sử Product có trường ImageUrl
                        Quantity = item.Quantity,
                        Discount = Convert.ToDouble(product.ProductDiscount),
                        Price = item.Price,
                        SubTotal = Convert.ToDecimal(item.Price * item.Quantity - (item.Price * item.Quantity * (decimal)product.ProductDiscount/100)),
                        Variant = productVariant.Variant.VariantName,
                        ColorName = productColor.Color.ColorName,
                        ColorCode = productColor.Color.ColorHexaValue,
                    });
                }
            }

            return response;
        }
        private string GetOrderStatusString(int status)
        {
            return status switch
            {
                1 => "Đang chờ xử lý",
                2 => "Đặt hàng thành công",
                3 => "Đang giao hàng",
                4 => "Đã giao hàng thành công",
                5 => "Đã nhận",
                6 => "Đã hủy",
                _ => "Không xác định"
            };
        }

    }
}
