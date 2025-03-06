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
        public MessageData CreateNewBrand(CreateNewBrandRequest request)
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
    }
}
