using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace my_cosmetic_store.Services
{
    public class UserAuthenticateService
    {
        private readonly UserRepository _userRepository;
        private readonly ApiOptions _apiOption;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserAuthenticateService(ApiOptions apiOption, DatabaseContext databaseContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = new UserRepository(apiOption, databaseContext, mapper);
            _apiOption = apiOption;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public object UserLogin(my_cosmetic_store.Dtos.Request.LoginRequest request)
        {
            try
            {
                var user = _userRepository.UserLogin(request);
                if (user == null)
                {
                    throw new ValidateError(1000, "Incorrect email or password");
                }
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiOption.Secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var claimList = new[]
                {
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.UserData, user.UserName),
                    new Claim(ClaimTypes.Sid, user.UserID.ToString()),
                };
                var token = new JwtSecurityToken(
                    issuer: _apiOption.ValidIssuer,
                    audience: _apiOption.ValidAudience,
                    expires: DateTime.Now.AddYears(1),
                    claims: claimList,
                    signingCredentials: credentials
                );
                var tokenByString = new JwtSecurityTokenHandler().WriteToken(token);
                return new
                {
                    token = tokenByString,
                    user = user
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object UserRegister(UserRegisterRequest request)
        {
            try
            {
                var user = _userRepository.FindByCondition(row => row.Email == request.Email).FirstOrDefault();
                if (user != null)
                {
                    throw new ValidateError(1001, "Email has been used");
                }
                var newUser = new User()
                {
                    UserName = request.UserName,
                    Password = UltilityFunction.CreateMD5(request.Password),
                    Phone = request.Phone,
                    Email = request.Email,
                };
                if(request.Email == "admin@gmail.com")
                {
                    newUser.Role = 1;
                }    
                _userRepository.Create(newUser);
                _userRepository.SaveChange();
                return newUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object UpdateUserInfors(int userID, UpdateUserInfor userUpdate)
        {
            var findUser = _userRepository.FindByCondition(x => x.UserID == userID).FirstOrDefault();
            if (findUser != null)
            {
                findUser.UserName = userUpdate.UserName;
                findUser.Phone = userUpdate.Phone;
                findUser.UpdatedDate = DateTime.Now;
                findUser.DateOfBirth = userUpdate.DateOfBirth;
                findUser.Gender = userUpdate.Gender;
                _userRepository.UpdateByEntity(findUser);
                return findUser;
            }
            return null;
        }
        public object UpdateUserAddress(int userID, UpdateUserAdress userUpdate)
        {
            var findUser = _userRepository.FindByCondition(x => x.UserID == userID).FirstOrDefault();
            if (findUser != null)
            {
                if (userUpdate.Address != null)
                {
                    findUser.Address = userUpdate.Address;
                }
                if (userUpdate.Phone != null)
                {
                    findUser.Phone = userUpdate.Phone;
                }
                findUser.UpdatedDate = DateTime.Now;
                _userRepository.UpdateByEntity(findUser);
                return findUser;
            }
            return null;
        }

        public object GetUserInfor(int UserID)
        {
            return _userRepository.FindByCondition(x => x.UserID == UserID).FirstOrDefault();
        }

        public object GetAllUserAdmin(int UserID)
        {
            return _userRepository.FindByCondition(x => x.UserID != UserID).Select(x => new
            {
                id = x.UserID,
                username = x.UserName,
                email = x.Email,
                phone = x.Phone,
                address = x.Address,
                avatar = x.Avatar,
                role = x.Role == 0 ? "Customer"
                : x.Role == 1 ? "Admin"
                : "Staff",
                dateOfBirth = x.DateOfBirth,
                gender = x.Gender == 1 ? "Nam" : "Nữ",
            }).ToList();
        }

        public object DeleteUser(int UserID)
        {
            var findUser = _userRepository.FindByCondition(x => x.UserID==UserID).FirstOrDefault();
            if(findUser != null)
            {
                _userRepository.DeleteByEntity(findUser);
                return findUser;
            }
            return null;
        }

        public object SetAdminRoleUser(int userId)
        {
            var findUser = _userRepository.FindByCondition(x => x.UserID == userId).FirstOrDefault();
            if (findUser != null)
            {
                findUser.Role = 1;
                _userRepository.UpdateByEntity(findUser);
                return findUser;
            }
            return null;
        }
        public object SetStaffRole(int userId)
        {
            var findUser = _userRepository.FindByCondition(x => x.UserID == userId).FirstOrDefault();
            if (findUser != null)
            {
                findUser.Role = 2;
                _userRepository.UpdateByEntity(findUser);
                return findUser;
            }
            return null;
        }
        public object SetCustomerRole(int userId)
        {
            var findUser = _userRepository.FindByCondition(x => x.UserID == userId).FirstOrDefault();
            if (findUser != null)
            {
                findUser.Role = 0;
                _userRepository.UpdateByEntity(findUser);
                return findUser;
            }
            return null;
        }
    }
}
