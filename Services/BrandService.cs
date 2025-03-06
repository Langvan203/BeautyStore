using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class BrandService
    {
        private readonly BrandRepository _brandRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandService(ApiOptions apiOptions,DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _brandRepository = new BrandRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public object CreateNewBrand(CreateNewBrandRequest createNewBrand)
        {
            var brand = _mapper.Map<Brand>(createNewBrand);
            _brandRepository.Create(brand);
            _brandRepository.SaveChange();
            return brand;
        }

        public object GetAllBrand()
        {
            return _brandRepository.FindAll();
        }
        public object GetBrandById(int id)
        {
            return _brandRepository.FindByCondition(x => x.BrandID == id);
        }
    }
}
