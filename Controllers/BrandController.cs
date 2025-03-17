using AutoMapper;
using Azure.Core;
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
    public class BrandController : BaseApiController<BrandController>
    {
        private readonly BrandService _brandService;
        private readonly IMapper _mapper;
        public BrandController(DatabaseContext context, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHost)
        {
            _mapper = mapper;
            _brandService = new BrandService(apiOptions, context, mapper, webHost);
        }

        [HttpGet("GetAllBrand")]
        [AllowAnonymous]
        public MessageData GetAllBrand()
        {
            try
            {
                var result = _brandService.GetAllBrand();
                return new MessageData { Data = result, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData { Data = ex.Message, Status = 0 };
            }
        }

        [HttpPost("CreateNewBrand")]
        [Authorize(Roles = "1")]
        public MessageData CreateNewBrand([FromForm]CreateNewBrandRequest request)
        {
            try
            {
                var newBrand = _brandService.CreateNewBrand(request);
                return new MessageData { Data = newBrand, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData { Data = ex.Message, Status = 0 };
            }
        
        }

        [HttpGet("Get-brand-top")]
        [AllowAnonymous]
        public MessageData GetBrandTop(int number)
        {
            try
            {
                var newBrand = _brandService.GetBrandTop(number);
                return new MessageData { Data = newBrand, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData { Data = ex.Message, Status = 0 };
            }
        }

        [HttpPut("Update-brand")]
        [Authorize(Roles = "1")]
        public MessageData UpdateBrand(UpdateBrandRequest request)
        {
            try
            {
                var newBrand = _brandService.UpdateBrand(request);
                return new MessageData { Data = newBrand, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData { Data = ex.Message, Status = 0 };
            }
        }

    }
}
