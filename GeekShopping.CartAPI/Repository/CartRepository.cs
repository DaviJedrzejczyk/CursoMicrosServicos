using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
    public class CartRepository : ICartRepository
    {

        private readonly SqlServerContext _context;
        private IMapper _mapper;

        public CartRepository(SqlServerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartVO> SaveOrUpdateCart(CartVO cartVO)
        {
            Cart cart = _mapper.Map<Cart>(cartVO);

            var cartDetailsFirstOrDefault = cart.CartDetails.FirstOrDefault();
            var cartVODetailsFirstOrDefaul = cartVO.CartDetails.FirstOrDefault();

            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartVODetailsFirstOrDefaul.ProductId);

            if (prod == null)
            {
                _context.Products.Add(cartDetailsFirstOrDefault.Product);
                await _context.SaveChangesAsync();
            }

            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

            if (cartHeader == null)
                await SaveNewCartHeader(cart, cartDetailsFirstOrDefault);
            else
                await SaveActualCartHeader(cart, cartDetailsFirstOrDefault, cartVODetailsFirstOrDefaul, cartHeader);

            return _mapper.Map<CartVO>(cart);
        }

        public Task<CartVO> FindCartByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCoupon(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFromCart(long cartDetailsId)
        {
            throw new NotImplementedException();
        }

        #region Métodos Privados
        private async Task SaveActualCartHeader(Cart cart, CartDetail cartDetailsFirstOrDefault, CartDetailVO cartVODetailsFirstOrDefaul, CartHeader cartHeader)
        {
            var cartDetail = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(p =>
                                    p.ProductId == cartVODetailsFirstOrDefaul.ProductId &&
                                    p.CartHeaderId == cartHeader.Id);
            if (cartDetail == null)
            {
                cartDetailsFirstOrDefault.CartHeaderId = cart.CartHeader.Id;
                cartDetailsFirstOrDefault.Product = null;

                _context.CartDetails.Add(cartDetailsFirstOrDefault);
                await _context.SaveChangesAsync();
            }
            else
            {
                cartDetailsFirstOrDefault.Product = null;
                cartDetailsFirstOrDefault.Count += cartDetail.Count;
                cartDetailsFirstOrDefault.Id = cartDetail.Id;
                cartDetailsFirstOrDefault.CartHeaderId = cartDetail.CartHeaderId;

                _context.CartDetails.Update(cartDetailsFirstOrDefault);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveNewCartHeader(Cart cart, CartDetail cartDetailsFirstOrDefault)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();

            cartDetailsFirstOrDefault.CartHeaderId = cart.CartHeader.Id;
            cartDetailsFirstOrDefault.Product = null;

            _context.CartDetails.Add(cartDetailsFirstOrDefault);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
