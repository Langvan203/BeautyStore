using AutoMapper;
using Azure.Core;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(ApiOptions apiOptions,DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = new UserRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        
    }
}
