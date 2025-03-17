using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
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

        public object GetCategoryByParentID(int parentID)
        {
            var listChildrenCategory = _childrenCategoryRepository.FindByCondition(x => x.ParentID == parentID).ToList();
            return listChildrenCategory;
        }

        public object CreateChildrenCategory(CreateNewChildrenCategory request)
        {
            if(request.ParentID == 0 || request.ParentID == null)
            {
                throw new Exception();
            }
            var newChildrenCategory = _mapper.Map<ChildrenCategory>(request);
            _childrenCategoryRepository.Create(newChildrenCategory);
            return newChildrenCategory;
        }

        public object DeleteChildrenCategory(int childrenCategoryID)
        {
            var findChildren = _childrenCategoryRepository.FindByCondition(x => x.ChildrenCategoryID == childrenCategoryID).FirstOrDefault();
            if (findChildren != null)
            {
                throw new Exception();
            }
            _childrenCategoryRepository.DeleteByEntity(findChildren);
            return findChildren;
        }
    }
}
