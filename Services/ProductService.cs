using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.IManagers;
using Interfaces.IServices;
using Models.DTOs;
namespace Services
{
    public class ProductService:IProductService
    {
        private readonly IProductManager _manager;

        public ProductService(IProductManager manager)
        {
            _manager = manager;
        }
        public async Task<Result<IList<ProductResponse>>> GetAllProducts() => await _manager.GetAllProducts();
        public async Task<Result<IList<ProductResponse>>> GetProductsByOrderId(int id) => await _manager.GetProductsByOrderId(id);
        public async Task<Result<ProductResponse>> GetProductById(int id)=>await _manager.GetProductById(id);
        public async Task<Result> AddAsync(CreateProduct product)=>await _manager.AddAsync(product);
        public async Task<Result> UpdateAsync(CreateProduct product)=> await _manager.UpdateAsync(product);
        public async Task<Result> DeleteAsync(int id)=> await _manager.DeleteAsync(id);




    }
}
