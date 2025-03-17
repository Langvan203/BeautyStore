using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Services;
using my_cosmetic_store.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace my_cosmetic_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController<ProductController>
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(DatabaseContext context, ApiOptions options, IMapper mapper, IWebHostEnvironment webhost)
        {
            _productService = new ProductService(options, context, mapper, webhost);
            _mapper = mapper;
        }

        [HttpPost("Create-product-images")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "1")]
        public MessageData CreateProduct([FromForm]CreateNewProductRequest request)
        {
            try
            {
                // Kiểm tra phải có đúng 1 ảnh chính
                if (request.MainImageIndex < 0 || request.MainImageIndex >= request.Files.Count)
                {
                    return new MessageData { Data = null, Des = "Chỉ số ảnh chính không hợp lệ." };
                }

                var product = _productService.CreateNewProduct(request);
                return new MessageData { Data = product, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("Get-all-product")]
        public MessageData GetAllProduct()
        {
            try
            {
                var products = _productService.GetAllProduct();
                return new MessageData { Data = products, Des = "success" };
            }
            catch(Exception ex)
            { 
                return NG(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet("Get-product-id")]
        public MessageData GetProduct(int id)
        {
            try
            {
                var products = _productService.GetProductById(id);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet("Find-product")]
        [AllowAnonymous]
        public MessageData FindProduct(string keyword)
        {
            try
            {
                var products = _productService.SearchProductName(keyword);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet("Find-product-price")]
        [AllowAnonymous]
        public MessageData FindProductPrice(FindProductPrice price)
        {
            try
            {
                var products = _productService.SearchProductPrice(price);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet("Find-product-brand")]
        [AllowAnonymous]
        public MessageData FindProductBrand(int brandId)
        {
            try
            {
                var products = _productService.GetProductByBrand(brandId);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpGet("Find-product-category")]
        [AllowAnonymous]
        public MessageData FindProductCategory(int CategoryId)
        {
            try
            {
                var products = _productService.GetProductByCategory(CategoryId);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet("Get-newest-product-top")]
        [AllowAnonymous]
        public MessageData GetNewestTop(int number)
        {
            try
            {
                var products = _productService.GetNewsetProductTop(number);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpDelete("Delete-product")]
        [Authorize(Roles = "1")]
        public MessageData DeleteProduct(int id)
        {
            try
            {
                var products = _productService.DeleteProduct(id);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPut("Update-product")]
        [Authorize(Roles = "1")]
        public MessageData UpdateProduct([FromForm]UpdateProductRequest request)
        {
            try
            {
                var products = _productService.UpdateProduct(request);
                return new MessageData { Data = products, Des = "success" };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
