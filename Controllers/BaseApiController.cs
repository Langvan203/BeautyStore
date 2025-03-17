using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_cosmetic_store.Utility;
using System.Security.Claims;

namespace my_cosmetic_store.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "1")]
    [ApiController]
    public class BaseApiController<T> : ControllerBase where T : BaseApiController<T>
    {
        public int UserIDLogined
        {
            get
            {
                return int.Parse(this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value);
            }
        }
        protected MessageData NG(Exception ex)
        {
            var response = new MessageData { Data = null, Code = "Error", Des = ex.Message, Status = -2 };
            if (ex.GetType().Name == "ValidateError")
            {
                var validateException = ex as ValidateError;
                response.Code = "ValidateError";
                response.ErrorCode = validateException.ErrorCode;
                response.Status = -1;
                return response;
            }
            response.Des = "Internal Server Error";
            return response;
        }
    }
}
