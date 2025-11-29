using Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace EcommerceJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _service.GetAllOrders();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            var result = await _service.GetOrdersByUserId(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

      
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _service.GetOrderById(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

      
        [HttpPost]
        [Authorize(Roles = "Customer")] 
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder dto)
        {
           
            //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            //dto.UserId = userId;
            var result = await _service.AddAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        
        [HttpPost("{id:int}/add-product")]
        [Authorize(Roles = "Customer")] 
        public async Task<IActionResult> AddProductToOrder(int id, [FromBody] CreateProduct product)
        {
            var result = await _service.AddProductToOrder(id, product);
            return result.Success ? Ok(result) : BadRequest(result);
        }

       
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int id,[FromBody] CreateOrder dto)
        {
            var result = await _service.UpdateAsync(id,dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }


    }
}