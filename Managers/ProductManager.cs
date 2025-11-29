using AutoMapper;
using DataAccess.Context;
using DataAccess.Entity;
using Interfaces.IManagers;
using Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Constants;
using Models.DTOs;
using Entity = DataAccess.Entity;
namespace Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IBaseRepository<Entity.Product> _repo;
        private readonly IBaseRepository<Entity.Order> _orderrepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public ProductManager(IBaseRepository<Entity.Product> repo, IMapper mapper, IBaseRepository<Entity.Order> orderrepo, AppDbContext context)
        {
            _repo = repo;
            _orderrepo = orderrepo;
            _mapper = mapper;
            _context=context;
        }
        public async Task<Result<IList<ProductResponse>>> GetAllProducts()
        {
            var products = await _repo.GetAllAsync();
            var data = _mapper.Map<IList<ProductResponse>>(products);

            return new Result<IList<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved",
                Data = data
            };

        }
        public async Task<Result<IList<ProductResponse>>> GetProductsByOrderId(int id)
        {
            //var orders = await _orderrepo.GetAllAsync();

            var order =await _context.Orders.AsQueryable()
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return new Result<IList<ProductResponse>>
                {
                    Success = false,
                    Message = ErrorConstants.InValid,
                    Data = null
                };
            }
            var products = order.Products;
            var data = _mapper.Map<IList<ProductResponse>>(products);
            return new Result<IList<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved",
                Data = data
            };
        }
        public async Task<Result<ProductResponse>> GetProductById(int id)
        {
            try
            {
                var product = await _repo.GetByIdAsync(id);

                if (product == null)
                {
                    return new Result<ProductResponse>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = null
                    };
                }

                var productdto = _mapper.Map<ProductResponse>(product);
                return new Result<ProductResponse>
                {
                    Success = true,
                    Message = "Products retrieved",
                    Data = productdto
                };
            }
            catch(Exception ex)
            {
                return new Result<ProductResponse>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

            }

        }
        public async Task<Result> AddAsync(CreateProduct product)
        {
            var newproduct = _mapper.Map<Product>(product);
            await _repo.AddAsync(newproduct);
            return new Result
            {
                Success = true,
                Message = "Added Successfully"
            };

        }
        public async Task<Result> UpdateAsync(int id,CreateProduct product)
        {
            var update = await _repo.GetByIdAsync(id);
            if (update == null)
            {
                return new Result
                {
                    Success = false,
                    Message = ErrorConstants.InValid
                };
            }
            _mapper.Map(product,update);
            await _repo.UpdateAsync(update);
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
                var product = await _repo.GetByIdAsync(id);
                if (product == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = ErrorConstants.InValid
                    };
                }
                await _repo.DeleteAsync(id);
                return new Result
                {
                    Success = true,
                    Message = "Deleted Successfully"
                };
            }
            catch(Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = ex.Message,
                    
                };
            }

        }
    }
}
