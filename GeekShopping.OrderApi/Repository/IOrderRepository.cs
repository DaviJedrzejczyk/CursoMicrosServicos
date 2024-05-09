using GeekShopping.OrderApi.Model;

namespace GeekShopping.OrderApi.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader orderHeader);
        Task UpdateOrderPayamentStatus(long orderHeaderId, bool payed);

    }
}
