using AutoMapper;
using Microsoft.EntityFrameworkCore;
using my_cosmetic_store.Dtos.Response;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrderService(ApiOptions apiOptions, IMapper mapper,DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _orderRepository = new OrderRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public object GetOrdersByUserIdAsync(int userId)
        {
            var orders = _orderRepository.FindByCondition(x => x.UserID == userId)
                .Include(o => o.Order_Items)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(oi => oi.ProductImages)
                .Include(o => o.Order_Items)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductVariants)
                    .ThenInclude(x => x.Variant)
                .Include(o => o.Order_Items)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductColors)
                    .ThenInclude(x => x.Color)
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return orders.Select(MapOrderToOrderResponseDto).ToList();
        }

        public object GetOrderByIdAsync(int orderId, int userId)
        {
                var order = _orderRepository.FindByCondition(x => x.OrderID == orderId && x.UserID == userId)
                    .Include(o => o.Order_Items)
                        .ThenInclude(oi => oi.Product)
                        .ThenInclude(oi => oi.ProductImages)
                    .Include(o => o.Order_Items)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductVariants)
                        .ThenInclude(x => x.Variant)
                    .Include(o => o.Order_Items)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductColors)
                        .ThenInclude(x => x.Color)
                .FirstOrDefault();

            if (order == null)
                throw new Exception("Không tìm thấy đơn hàng");

            return MapOrderToOrderResponseDto(order);
        }

        public object GetChartOrder(int year)
        {
            var orders = _orderRepository.FindByCondition(x => x.OrderDate.Year == year)
                .Include(x => x.Order_Items)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(p => p.Category)
                .Include(x => x.User).ToList();


            var months = MonthConverter.Months;

            var totalProducts = orders.SelectMany(x => x.Order_Items).Count();
            var orderByCategory = orders.SelectMany(x => x.Order_Items)
                                        .Select(x => x.Product)
                                        .Select(x => x.Category)
                                        .GroupBy(x => new {x.CategoryID, x.Name}).Select(x => new
                                        {
                                            infor = x.Key,
                                            total = x.Count(),
                                        }).ToList();
            var totalOrders = orders.ToList().Count;
            var totalRevenue = orders.Sum(x => x.Total_Amount);
            var totalUsers = orders.GroupBy(x => x.UserID).Count();
            var monthInOrderYear = orders.Select(x => x.OrderDate.Month).ToList();
            var order_items = orders.SelectMany(x => x.Order_Items);
            var categoryProduct = order_items.Select(x => new
            {
                categoryId = x.Product.CategoryID,
                categoryName = x.Product.Category.Name,
            }).GroupBy(g => g.categoryId).ToList();
            var recentOrders = orders.Select((item,index) => new
            {
                id = index+1,
                name = item.User.UserName,
                orderDate = item.OrderDate,
                totalAmount = item.Total_Amount,
                status = GetOrderStatusString(item.Status)
            });
            List<MonthlySales> monthlySales = new List<MonthlySales>();
            foreach(var item in months)
            {
                MonthlySales monthly = new MonthlySales();
                if(monthInOrderYear.Contains(item.Month))
                {
                    monthly.month = item.MonthName;
                    monthly.sales = orders.Where(x => x.OrderDate.Month == item.Month && x.OrderDate.Year == year).SelectMany(x => x.Order_Items).Count();
                }    
                else
                {
                    monthly.month = item.MonthName;
                    monthly.sales = 0;
                }    
                monthlySales.Add(monthly);
                
            }
            List<CategorySales> categorySales = new List<CategorySales>();
            foreach (var item in orderByCategory)
            {
                CategorySales category = new CategorySales
                {
                    //CategoryID = item.infor.CategoryID,
                    name = item.infor.Name,
                    //sales = item.total,
                    value = (decimal)item.total*100/totalProducts
                };
                categorySales.Add(category);
            }
            return new
            {
                totalOrders = totalOrders,
                totalProducts = totalProducts,
                totalRevenue = totalRevenue,
                totalUsers = totalUsers,
                recentOrders = recentOrders,
                monthlySales = monthlySales,
                categoryDistribution = categorySales
            };
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
                    var variant = item.ProductVariant.Variant;
                    var color = item.ProductColor.Color;
                    var mainImage = product.ProductImages.Where(x => x.Is_primary == 1).Select(x => x.ImageUrl).FirstOrDefault();
                    response.Items.Add(new OrderItemDetailDto
                    {
                        OrderItemID = item.OrderItemID,
                        ProductID = item.ProductID,
                        ProductName = product?.ProductName, // Giả sử Product có trường Name
                        ProductImage = mainImage, // Giả sử Product có trường ImageUrl
                        Discount = Convert.ToDouble(product.ProductDiscount),
                        Quantity = item.Quantity,
                        Price = item.Price,
                        SubTotal = Convert.ToDecimal(item.Price * item.Quantity - (item.Price * item.Quantity * (decimal)product.ProductDiscount / 100)),
                        Variant = variant.VariantName,
                        PriceOfVariant = item.PriceOfVariant,
                        ColorName = color.ColorName,
                        ColorCode = color.ColorHexaValue
                    });
                }
            }

            return response;
        }

        public object GetAllOrderAdmin()
        {
            var orderAll =  _orderRepository.FindAll().Include(x => x.User).Select(x => new
            {
                orderid = x.OrderID,
                date = x.OrderDate,
                totalAmount = x.Total_Amount,
                status = x.Status,
                phone = x.PhoneNumber,
                userx = x.User.UserName,
            }).ToList();
            var getAllOrderAdmin = orderAll.Select(x => new
            {
                id = x.orderid,
                date = x.date,
                totalAmount = x.totalAmount,
                status = GetOrderStatusString(x.status),
                phone = x.phone,
                userName = x.userx
            }).ToList();
            return getAllOrderAdmin;
        }


        public object CancelledOrderByUser(int orderId, int UserId)
        {
            var findOrder = _orderRepository.FindByCondition(x => x.OrderID == orderId && x.UserID == UserId).Include(x => x.HistoryOder).FirstOrDefault();
            if (findOrder != null)
            {
                findOrder.Status = 6;
                var newStatus = GetOrderStatusString(6);
                var historyOrderFind = findOrder.HistoryOder.Where(x => x.Title.Equals(newStatus)).FirstOrDefault();
                if (historyOrderFind == null)
                {
                    HistoryOder newHistory = new HistoryOder
                    {
                        OrderID = orderId,
                        UpdatedDate = DateTime.UtcNow,
                        Title = newStatus,
                    };
                    findOrder.HistoryOder.Add(newHistory);
                }
                _orderRepository.UpdateByEntity(findOrder);
                return new
                {
                    timeLine = findOrder.HistoryOder.Select(x => new
                    {
                        Title = x.Title,
                        date = x.UpdatedDate,
                    }).ToList()
                };
            }
            return null;
        }

        public object UpdateOrderStatus(int orderId, int status)
        {
            var findOrder = _orderRepository.FindByCondition(x => x.OrderID == orderId).Include(x => x.HistoryOder).FirstOrDefault();
            if (findOrder != null)
            {
                findOrder.Status = status;
                var newStatus = GetOrderStatusString(status);
                var historyOrderFind = findOrder.HistoryOder.Where(x => x.Title.Equals(newStatus)).FirstOrDefault();
                if(historyOrderFind == null)
                {
                    HistoryOder newHistory = new HistoryOder
                    {
                        OrderID = orderId,
                        UpdatedDate = DateTime.UtcNow,
                        Title = newStatus,
                    };
                    findOrder.HistoryOder.Add(newHistory);
                }
                _orderRepository.UpdateByEntity(findOrder);
                return new
                {
                    timeLine = findOrder.HistoryOder.Select(x => new
                    {
                        Title = x.Title,
                        date = x.UpdatedDate,
                    }).ToList()
                };
            }
            return null;
        }

        public object GetOrderHistory(int orderId)
        {
            var findOrder = _orderRepository.FindByCondition(x => x.OrderID == orderId)
                                            .Include(x => x.Order_Items)
                                                .ThenInclude(x => x.Product)
                                                .ThenInclude(x => x.ProductVariants)
                                                .ThenInclude(x => x.Variant)
                                            .Include(x => x.Order_Items)
                                                .ThenInclude(x => x.Product)
                                                .ThenInclude(x => x.ProductImages)
                                            .Include(o => o.Order_Items)
                                                .ThenInclude(x => x.Product)
                                                .ThenInclude(x => x.ProductColors)
                                                .ThenInclude(x => x.Color)
                                            .Include(x => x.HistoryOder)
                                            .FirstOrDefault();
            
                
            if (findOrder != null)
            {
                var InOrderItems = findOrder.Order_Items;
                var timeLine = findOrder.HistoryOder;
                List<ProductInOrderItem> productInOrderItems = new List<ProductInOrderItem>();
                List<TimeLineOrder> timeLineOrder = new List<TimeLineOrder>();
                foreach (var item in InOrderItems)
                {
                    var products = item.Product;
                    ProductInOrderItem productInOrderItem = new ProductInOrderItem
                    {
                        id = products.ProductID,
                        name = products.ProductName,
                        price = item.PriceOfVariant,
                        discount = products.ProductDiscount,
                        finalPrice = item.Price,
                        quantity = item.Quantity,
                        image = products.ProductImages.Where(x => x.Is_primary == 1).Select(x => x.ImageUrl).FirstOrDefault(),
                        variant = item.ProductVariant.Variant.VariantName,
                        ColorName = item.ProductColor.Color.ColorName,
                        ColorCode = item.ProductColor.Color.ColorHexaValue
                    };
                    productInOrderItems.Add(productInOrderItem);

                }
                foreach (var item in timeLine)
                {
                    TimeLineOrder timeLineOrder1 = new TimeLineOrder
                    {
                        status = GetOrderStatusString(findOrder.Status),
                        date = item.UpdatedDate,
                        description = item.Title
                    };
                    timeLineOrder.Add(timeLineOrder1);
                }
                return new
                {
                    id = orderId,
                    date = findOrder.CreatedDate,
                    status = GetOrderStatusString(findOrder.Status),
                    statusCode = findOrder.Status,
                    shippingMethod = findOrder.ShippingMethod,
                    paymentMethod = findOrder.PaymentMethod,
                    shippingAdress = findOrder.ShippingAdress,
                    phoneNumber = findOrder.PhoneNumber,
                    receiverName = findOrder.ReceiverName,
                    items = productInOrderItems,
                    subtotal = findOrder.Total_Amount,
                    shipping = (findOrder.Total_Amount > 500000 ? 30000: 0),
                    total = (findOrder.Total_Amount > 500000 ? findOrder.Total_Amount+30000 : findOrder.Total_Amount),
                    timeLine = timeLineOrder
                };
            }
            return null;
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
