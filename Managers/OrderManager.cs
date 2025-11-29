using AutoMapper;
using Interfaces.IManagers;
using Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Constants;
using Models.DTOs;
using Entity = DataAccess.Entity;
using DataAccess.Context;
using Microsoft.AspNetCore.Identity;
namespace Managers
{
    public class OrderManager : IOrderManager
    {
       
        private readonly IBaseRepository<Entity.Order> _orderrepo;
        private readonly UserManager<Entity.AppUser> _usermanager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderManager(IBaseRepository<Entity.AppUser> userrepo, IMapper mapper, IBaseRepository<Entity.Order> orderrepo,AppDbContext context, UserManager<Entity.AppUser> usermanager)
        {

            _orderrepo = orderrepo;
            _mapper = mapper;
            _context = context;
            _usermanager=usermanager;

        }
        public async Task<Result<IList<OrderResponse>>> GetAllOrders()
        {
            var orders = await _orderrepo.GetAllAsync();
            var data = _mapper.Map<IList<OrderResponse>>(orders);

            return new Result<IList<OrderResponse>>
            {
                Success = true,
                Message = "Orders retrieved",
                Data = data
            };

        }
        public async Task<Result<IList<OrderResponse>>> GetOrdersByUserId(string id)
        {
           
            var orderList = await _context.Orders.AsQueryable()
                .Where(o => o.UserId == id)
                .Include(o => o.Products)
                .Include(o => o.AppUser)
                .ToListAsync();

            if (!orderList.Any())
            {
                return new Result<IList<OrderResponse>>
                {
                    Success = false,
                    Message = ErrorConstants.InValid,
                    Data = null
                };
            }

            var data = _mapper.Map<IList<OrderResponse>>(orderList);
            return new Result<IList<OrderResponse>>
            {
                Success = true,
                Message = "Orders retrieved",
                Data = data
            };
        }
        public async Task<Result<OrderResponse>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderrepo.GetByIdAsync(id);
                if (order == null)
                {
                    return new Result<OrderResponse>
                    {
                        Success = false,
                        Message = ErrorConstants.InValid,
                        Data = null
                    };
                }
                var orderdto = _mapper.Map<OrderResponse>(order);
                return new Result<OrderResponse>
                {
                    Success = true,
                    Message = "Products retrieved",
                    Data = orderdto
                };
            }
            catch (Exception ex)
            {
                return new Result<OrderResponse>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<Result> AddProductToOrder(int id, CreateProduct product)
        {
            var order = await _orderrepo.GetByIdAsync(id);
            if (order == null)
            {
                return new Result
                {
                    Success = false,
                    Message = ErrorConstants.InValid
                };
            }
            var newproduct = _mapper.Map<Entity.Product>(product);
            order.Products ??= new List<Entity.Product>();
            order.Products.Add(newproduct);
            await _orderrepo.UpdateAsync(order);
            return new Result
            {
                Success = true,
                Message = "Added Successfully"
            };

        }
        public async Task<Result> AddAsync(CreateOrder order)
        {
            var neworder = _mapper.Map<Entity.Order>(order);
            await _orderrepo.AddAsync(neworder);
            return new Result
            {
                Success = true,
                Message = "Added Successfully"
            };

        }
        public async Task<Result> UpdateAsync(int id,CreateOrder order)
        {

            

            var update = await _orderrepo.GetByIdAsync(id);
            if (update == null)
            {
                return new Result
                {
                    Success = false,
                    Message = ErrorConstants.InValid
                };
            }
            var user = await _usermanager.FindByIdAsync(order.UserId);
            if(user==null)
            {
                return new Result
                {
                    Success = false,
                    Message = ErrorConstants.InValid
                };
            }
            order.UserId = user.Id;
            _mapper.Map(order,update);
            await _orderrepo.UpdateAsync(update);
            return new Result
            {
                Success = true,
                Message = "Updated Successfully"
            };

        }
        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var order = await _orderrepo.GetByIdAsync(id);
                if (order == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = ErrorConstants.InValid
                    };
                }
                await _orderrepo.DeleteAsync(id);
                return new Result
                {
                    Success = true,
                    Message = "Deleted Successfully"
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = ex.Message
                   
                };
            }

        }
    }
}
