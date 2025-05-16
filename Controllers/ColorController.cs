using AutoMapper;
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
    public class ColorController : BaseApiController<ColorController>
    {
        private readonly ColorService _colorService;
        private readonly IMapper _mapper;
        public ColorController(DatabaseContext context, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHost)
        {
            _mapper = mapper;
            _colorService = new ColorService(apiOptions, context, mapper, webHost);
        }
        [HttpGet("Get-all-color-admin")]
        [Authorize(Roles = "1,2")]
        public MessageData GetAllColorAdmin()
        {
            try
            {
                var Colorype = _colorService.GetAllColorTypeAdmin();
                return new MessageData { Data = Colorype, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpPost("AddNewColor")]
        [Authorize(Roles = "1,2")]
        public MessageData AddNewColor(ColorDto Color)
        {
            try
            {
                var Colorype = _colorService.AddNewColor(Color);
                return new MessageData { Data = Colorype, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpDelete("DeleteColor")]
        [Authorize(Roles = "1,2")]
        public MessageData DeleteColor(int ColorId)
        {
            try
            {
                var Colorype = _colorService.DeleteColor(ColorId);
                return new MessageData { Data = Colorype, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
