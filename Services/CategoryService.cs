using AutoMapper;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class CategoryService 
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryService(CategoryRepository categoryRepository, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepository = categoryRepository;
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
    }
}
