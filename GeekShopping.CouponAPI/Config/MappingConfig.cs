using GeekShopping.CouponAPI.Data.ValueObjects;
using GeekShopping.CouponAPI.Model;

namespace GeekShopping.CouponAPI.Config
{
    public class MappingConfig : AutoMapper.Profile
    {
        public MappingConfig()
        {
            CreateMap<CouponVO, Coupon>().ReverseMap();
        }
    }
}
