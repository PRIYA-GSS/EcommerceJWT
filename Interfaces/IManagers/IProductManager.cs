using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;
namespace Interfaces.IManagers
{
    public interface IProductManager
    {
        Task<Result<IList<ProductResponse>>> GetAllProducts();
        Task<Result<IList<ProductResponse>>> GetProductsByOrderId(int id);
        Task<Result<ProductResponse>> GetProductById(int id);
        Task<Result> AddAsync(CreateProduct product);
        Task<Result> UpdateAsync(int id,CreateProduct product);
        Task<Result> DeleteAsync(int id);
    }
}
