using AutoMapper;
using Azure.Core;
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
    public class CategoryController : BaseApiController<CategoryController>
    {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(DatabaseContext context, ApiOptions options, IMapper mapper, IWebHostEnvironment webhost)
        {
            _mapper = mapper;
            _categoryService = new CategoryService(options, context, mapper, webhost);
        }


        [HttpGet("Get-all-categories")]
        [AllowAnonymous]
        public MessageData GetAllCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategory();
                return new MessageData { Data = categories, Status = 1 };
            }
            catch(Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet("Get-top-categories")]
        [AllowAnonymous]
        public MessageData GetTopCategories(int number)
        {
            try
            {
                var categories = _categoryService.GetTopCategory(number);
                return new MessageData { Data = categories, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost("Create-new-category")]
        [Authorize(Roles = "1")]
        public MessageData CreateNewCategory([FromForm]CreateNewCategoryRequest request)
        {
            try
            {
                var categories = _categoryService.CreateNewCategory(request);
                return new MessageData { Data = categories, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpDelete("Delete-category-admin")]
        [Authorize(Roles = "1")]
        public MessageData DeleteCategory(int categoryid)
        {
            try
            {
                var categories = _categoryService.DeleteCategory(categoryid);
                return new MessageData { Data = categories, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPut("Update-category")]
        [Authorize(Roles = "1")]
        public MessageData UpdateCategory([FromForm]UpdateCategoryRequest request)
        {
            try
            {
                var category = _categoryService.UpdateCategory(request);
                return new MessageData { Data = category, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
