using OrderApp.Entities;

namespace OrderApp.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrderAsync();
        Task<Order> GetOrderByIdAsync(string id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order); 
        Task<Order> DeleteOrderAsync(string id); 
    }
}
