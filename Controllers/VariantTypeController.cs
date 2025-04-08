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
    public class VariantTypeController : BaseApiController<VariantTypeController>
    {
        private readonly VariantTypeServices _variantTypeServices;
        private readonly IMapper _mapper;
        public VariantTypeController(DatabaseContext context, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHost)
        {
            _mapper = mapper;
            _variantTypeServices = new VariantTypeServices(apiOptions, context, mapper, webHost);
        }
        [HttpGet("Get-all-variantype-admin")]
        [Authorize(Roles = "1")]
        public MessageData GetAllVariantypeAdmin()
        {
            try
            {
                var variantype = _variantTypeServices.GetAllVariantTypeAdmin();
                return new MessageData { Data = variantype, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpPost("AddNewVariant")]
        [Authorize(Roles = "1")]
        public MessageData AddNewVariant(string variant)
        {
            try
            {
                var variantype = _variantTypeServices.AddNewVariant(variant);
                return new MessageData { Data = variantype, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpDelete("DeleteVariant")]
        [Authorize(Roles = "1")]
        public MessageData DeleteVariant(int variantId)
        {
            try
            {
                var variantype = _variantTypeServices.DeleteVariant(variantId);
                return new MessageData { Data = variantype, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }


    }
}
