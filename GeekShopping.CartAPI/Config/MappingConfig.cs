using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;

namespace GeekShopping.CartAPI.Config
{
    public class MappingConfig : AutoMapper.Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductVO, Product>().ReverseMap();
            CreateMap<CartVO, Cart>().ReverseMap();
            CreateMap<CartDetailVO, CartDetail>().ReverseMap();
            CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
        }
    }
}
