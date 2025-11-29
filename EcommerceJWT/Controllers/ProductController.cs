using Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace EcommerceJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _service.GetAllProducts();
            return result.Success ? Ok(result) : BadRequest(result);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _service.GetProductById(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

       
        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetProductsByOrderId(int orderId)
        {
            var result = await _service.GetProductsByOrderId(orderId);
            return result.Success ? Ok(result) : NotFound(result);
        }

     
        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> AddProduct([FromBody] CreateProduct dto)
        {
            var result = await _service.AddAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateProduct(int id,[FromBody] CreateProduct dto)
        {
            var result = await _service.UpdateAsync(id,dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }

}