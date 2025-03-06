using AutoMapper;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class ChildrenCategoryService
    {
        private readonly ChildrenCategoryRepository _childrenCategoryRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChildrenCategoryService(ChildrenCategoryRepository childrenCategoryRepository, ApiOptions apiOptions, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _childrenCategoryRepository = childrenCategoryRepository;
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
    }
}
