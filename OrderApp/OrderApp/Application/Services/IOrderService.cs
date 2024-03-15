using OrderApp.Entities;

namespace OrderApp.Application.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetALlOrderAsync();
        Task<Order> CreateOrderAsync(Order order);
    }
}
