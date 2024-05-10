using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IRabbitMQMessageSender _messageSender;

        public CartController(ICartRepository repository, IRabbitMQMessageSender messageSender, ICouponRepository couponRepository)
        {
            _cartRepository = repository;
            _messageSender = messageSender;
            _couponRepository = couponRepository;
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartVO>> FindById(string id)
        {

            var cart = await _cartRepository.FindCartByUserId(id);
            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);
            if (!status) return BadRequest();
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO vo)
        {
            var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string id)
        {
            var status = await _cartRepository.RemoveCoupon(id);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO checkoutHeaderVO)
        {
            string token = await HttpContext.GetTokenAsync("access_token");

            if(checkoutHeaderVO?.UserId == null) return BadRequest();

            var cart = await _cartRepository.FindCartByUserId(checkoutHeaderVO.UserId);
            if (cart == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(checkoutHeaderVO.CouponCode))
            {
                CouponVO couponVO = await _couponRepository.GetCoupon(checkoutHeaderVO.CouponCode, token);

                if (couponVO.DiscountAmount != checkoutHeaderVO.DiscountTotal)
                {
                    return StatusCode(412);
                }
            }

            checkoutHeaderVO.CartDetails = cart.CartDetails;
            checkoutHeaderVO.DateTime = DateTime.Now;

            _messageSender.SendMessage(checkoutHeaderVO, "checkouqueue");

            await _cartRepository.ClearCart(checkoutHeaderVO.UserId);

            return Ok(checkoutHeaderVO);
        }
    }
}
