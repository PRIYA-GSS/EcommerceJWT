using Interfaces.IManagers;
using Interfaces.IServices;
using Models.DTOs;
namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderManager _manager;
        public OrderService(IOrderManager manager)
        {
            _manager = manager;
        }
        public async Task<Result<IList<OrderResponse>>> GetAllOrders() => await _manager.GetAllOrders();
        public async Task<Result<IList<OrderResponse>>> GetOrdersByUserId(string id) => await _manager.GetOrdersByUserId(id);
        public async Task<Result<OrderResponse>> GetOrderById(int id) => await _manager.GetOrderById(id);
        public async Task<Result> AddAsync(CreateOrder order) => await _manager.AddAsync(order);

        public async Task<Result> AddProductToOrder(int id, CreateProduct product) => await _manager.AddProductToOrder(id, product);
        public async Task<Result> UpdateAsync(CreateOrder order) => await _manager.UpdateAsync(order);
        public async Task<Result> DeleteAsync(int id) => await _manager.DeleteAsync(id);

    }
}
