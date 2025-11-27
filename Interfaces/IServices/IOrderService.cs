using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;

namespace Interfaces.IServices
{
    public interface IOrderService
    {
        Task<Result<IList<OrderResponse>>> GetAllOrders();
        Task<Result<IList<OrderResponse>>> GetOrdersByUserId(string id);
        Task<Result<OrderResponse>> GetOrderById(int id);
        Task<Result> AddAsync(CreateOrder order);
        Task<Result> AddProductToOrder(int id, CreateProduct product);
        Task<Result> UpdateAsync(CreateOrder order);
        Task<Result> DeleteAsync(int id);
    }
}
