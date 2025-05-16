using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class UserController : BaseApiController<UserController>
    {
        private readonly UserAuthenticateService _userAuthenticateService;
        public UserController(DatabaseContext databaseContext, IMapper mapper, ApiOptions apiConfig, IWebHostEnvironment webHost)
        {
            _userAuthenticateService = new UserAuthenticateService(apiConfig, databaseContext, mapper, webHost);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("UserLogin")]
        public MessageData UserLogin(LoginRequest request)
        {
            try
            {
                var res = _userAuthenticateService.UserLogin(request);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("UserRegister")]
        public MessageData UserRegister(UserRegisterRequest request)
        {
            try
            {
                var res = _userAuthenticateService.UserRegister(request);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPut]
        [Route("UpdateUserInfor")]
        [Authorize]
        public MessageData UpdateUserInfor([FromBody] UpdateUserInfor request)
        {
            try
            {
                var res = _userAuthenticateService.UpdateUserInfors(UserIDLogined, request);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpPut]
        [Route("UpdateUserAddress")]
        [Authorize]
        public MessageData UpdateUserAddress(UpdateUserAdress request)
        {
            try
            {
                var res = _userAuthenticateService.UpdateUserAddress(UserIDLogined, request);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpGet]
        [Route("GetUserInfo")]
        [Authorize]
        public MessageData GetUserInfo()
        {
            try
            {
                var res = _userAuthenticateService.GetUserInfor(UserIDLogined);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpGet("GetAllUserAdmin")]
        [Authorize(Roles = "1")]
        public MessageData GetAllUserAdmin()
        {
            try
            {
                var res = _userAuthenticateService.GetAllUserAdmin(UserIDLogined);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "1")]
        public MessageData DeleteUser(int UserID)
        {
            try
            {
                var res = _userAuthenticateService.DeleteUser(UserID);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost("SetAdminRole")]
        [Authorize(Roles = "1")]
        public MessageData SetAdminRole(int UserID)
        {
            try
            {
                var res = _userAuthenticateService.SetAdminRoleUser(UserID);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost("SetStaffRole")]
        [Authorize(Roles = "1")]
        public MessageData SetStaffRole(int UserID)
        {
            try
            {
                var res = _userAuthenticateService.SetStaffRole(UserID);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpPost("SetCustomerRole")]
        [Authorize(Roles = "1")]
        public MessageData SetCustomerRole(int UserID)
        {
            try
            {
                var res = _userAuthenticateService.SetCustomerRole(UserID);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
