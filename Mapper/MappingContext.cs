using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Dtos.Response;
using my_cosmetic_store.Models;

namespace my_cosmetic_store.Mapper
{
    public class MappingContext : Profile
    {
        public MappingContext()
        {
            // Create request mapping
            CreateMap<CreateNewBrandRequest, Brand>();
            CreateMap<CreateNewCarRequest, Cart>();
            CreateMap<CreateNewCategoryRequest, Category>();
            CreateMap<CreateNewChildrenCategory, ChildrenCategory>();
            CreateMap<UpdateUserInfor, User>();
            CreateMap<UpdateUserAdress, User>();
            CreateMap<CreateNewPaymentMethod, PaymentMethod>();
            // Create response mapping
            CreateMap<Cart, CartResponseDto>();
            CreateMap<ColorDto, Color>();
        }
    }
}
