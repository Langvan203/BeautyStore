using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class ColorService
    {
        private readonly ColorRepository _colorRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ColorService(ApiOptions apiOptions, DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _colorRepository = new ColorRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public object GetAllColorTypeAdmin()
        {
            return _colorRepository.FindAll().Select(x => new
            {
                id = x.ColorID,
                hexaValue = x.ColorHexaValue,
                name = x.ColorName,
            }).ToList();
        }

        public object AddNewColor(ColorDto color)
        {
            var findColor = _colorRepository.FindByCondition(x => x.ColorHexaValue.Equals(color.ColorHexaValue)).FirstOrDefault();
            if (findColor == null)
            {
                Color newColor = new Color
                {
                    ColorHexaValue = color.ColorHexaValue,
                    StockOfColor = 0,
                    ColorName = color.ColorName
                };
                _colorRepository.Create(newColor);
                return newColor;
            }
            throw new Exception("Color has already exsits");
        }
        public object DeleteColor(int colorID)
        {
            var findColor = _colorRepository.FindByCondition(x => x.ColorID == colorID).FirstOrDefault();
            if (findColor == null)
            {
                throw new Exception("Color has already exsits");
            }
            else
            {
                _colorRepository.DeleteByEntity(findColor);
                return findColor;
            }

        }
    }
}
