using GeekShopping.OrderApi.Model;
using GeekShopping.OrderApi.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Repository
{
    public class OrderRepository : IOrderRepository
    {

        private readonly DbContextOptions<SqlServerContext> _context;

        public OrderRepository(DbContextOptions<SqlServerContext> context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            if(orderHeader == null)
            {
                return false;
            }
            await using var _db = new SqlServerContext(_context);
            _db.Headers.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPayamentStatus(long orderHeaderId, bool status)
        {
            await using var _db = new SqlServerContext(_context);
            var header = await _db.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
            if(header != null)
            {
                header.PaymentStatus = status;
                await _db.SaveChangesAsync();
            }
        }
    }
}
