using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        private IMapper _mapper;
        public UserRepository(ApiOptions apiOptions, DatabaseContext context, IMapper mapper) : base(apiOptions, context)
        {
            _mapper = mapper;
        }
        public User UserLogin(my_cosmetic_store.Dtos.Request.LoginRequest userLoginRequest)
        {
            try
            {
                var passwordByMD5 = UltilityFunction.CreateMD5(userLoginRequest.Password);
                return Model.Where(row => row.Email == userLoginRequest.Email && row.Password == passwordByMD5).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
