using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;
using System.Runtime.InteropServices.Marshalling;

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
            if(createNewBrand.thumbNail != null)
            {
                var fileName = $"{Guid.NewGuid()}-{createNewBrand.thumbNail.FileName}";
                using (var filestream = File.Create(Path.Combine(_webHostEnvironment.WebRootPath + "\\images\\brands\\" + fileName)))
                {
                    createNewBrand.thumbNail.CopyTo(filestream);
                    filestream.Flush();
                }
                brand.thumbNail = "\\images\\brands\\" + fileName;
            }
            _brandRepository.Create(brand);
            _brandRepository.SaveChange();
            return brand;
        }

        public object GetAllBrand()
        {
            return _brandRepository.FindAll();
        }
        public object GetAllBrandAdmin()
        {
            return _brandRepository.FindAll().Select(x => new
            {
                id = x.BrandID,
                name = x.Name,
                description = x.Description,
                thumbnail = x.thumbNail,
            });
        }
        public object GetBrandById(int id)
        {
            return _brandRepository.FindByCondition(x => x.BrandID == id).FirstOrDefault();
        }

        public object DeleteBrand(int brandId)
        {
            var findBrand = _brandRepository.FindByCondition(x => x.BrandID == brandId).FirstOrDefault();
            if (findBrand != null)
            {
                throw new Exception();
            }
            _brandRepository.DeleteByEntity(findBrand);
            return findBrand;
        }

        public object GetBrandTop(int number)
        {
            var findBrandTop = _brandRepository.GetTopItems(number);
            return findBrandTop;
        }

        public object UpdateBrand(UpdateBrandRequest request)
        {
            var findBrand = _brandRepository.FindByCondition(x => x.BrandID == request.BrandId).FirstOrDefault();
            if (findBrand != null)
            {
                findBrand.Name = request.Name;
                findBrand.Description = request.Description;
                findBrand.UpdatedDate = DateTime.UtcNow;
                if(request.thumbNail != null)
                {
                    if(findBrand.thumbNail != null)
                    {
                        File.Delete(_webHostEnvironment.WebRootPath + findBrand.thumbNail);
                    }
                    var fileName = $"{Guid.NewGuid()}-{request.thumbNail.FileName}";
                    using (var filestream = File.Create(Path.Combine(_webHostEnvironment.WebRootPath + "\\images\\brands\\" + fileName)))
                    {
                        request.thumbNail.CopyTo(filestream);
                        filestream.Flush();
                    }
                    findBrand.thumbNail = "\\images\\brands\\" + fileName;
                    _brandRepository.UpdateByEntity(findBrand);
                }    
                return findBrand;
            }
            return null;
        }
    }
}
