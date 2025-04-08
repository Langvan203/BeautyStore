using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class VariantTypeServices
    {
        private readonly VariantTypeRepository _variantTypeRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VariantTypeServices(ApiOptions apiOptions, DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _variantTypeRepository = new VariantTypeRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public object GetAllVariantTypeAdmin()
        {
            return _variantTypeRepository.FindAll().Select(x => new
            {
                id = x.VariantId,
                name = x.VariantName
            }).ToList();
        }

        public object AddNewVariant(string variant)
        {
            var findVariant = _variantTypeRepository.FindByCondition(x => x.VariantName.Equals(variant)).FirstOrDefault();
            if (findVariant == null)
            {
                Variant newVariantAdd = new Variant
                {
                    VariantName = variant,
                    StockOfVariant = 0,
                    PriceOfVariant = 0,
                };
                _variantTypeRepository.Create(newVariantAdd);
                return newVariantAdd;
            }
            throw new Exception("Variant has already exsits");
        }
        public object DeleteVariant(int variantId)
        {
            var findVariant = _variantTypeRepository.FindByCondition(x => x.VariantId == variantId).FirstOrDefault();
            if (findVariant == null)
            {
                throw new Exception("Variant has already exsits");
            }
            else
            {
                _variantTypeRepository.DeleteByEntity(findVariant);
                return findVariant;
            }
            
        }
    }
}
