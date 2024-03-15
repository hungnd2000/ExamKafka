using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderApp.Application.Repositories;
using OrderApp.Data;
using OrderApp.Entities;

namespace OrderApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(OrderDbContext dbContext, ILogger<OrderRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                var result = await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<Order> DeleteOrderAsync(string id)
        {
            try
            {
                var orderDelete = await GetOrderByIdAsync(id);
                var result = _dbContext.Orders.Remove(orderDelete);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<Order>> GetAllOrderAsync()
        {
            try
            {
                return await _dbContext.Orders.ToListAsync();
            }catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public Task<Order> GetOrderByIdAsync(string id)
        {
            try
            {
                return _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                var orderUpdate = await GetOrderByIdAsync(order.Id);
                orderUpdate.ProductId = order.ProductId;
                orderUpdate.Amount = order.Amount;
                orderUpdate.Status = order.Status;
                orderUpdate.CreatedAt = order.CreatedAt;
                var result = _dbContext.Orders.Update(orderUpdate);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
