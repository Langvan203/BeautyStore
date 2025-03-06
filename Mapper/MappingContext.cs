using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;

namespace my_cosmetic_store.Mapper
{
    public class MappingContext : Profile
    {
        public MappingContext()
        {
            CreateMap<CreateNewBrandRequest, Brand>();
        }
    }
}
