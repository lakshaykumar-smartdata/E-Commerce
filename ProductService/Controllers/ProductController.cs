using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Dto;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("AddUpdateProduct")]
        public async Task<IActionResult> CreateOrUpdateProduct([FromBody] ProductCreateRequestDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid product data");
            }

            try
            {
                var product = await _productService.CreateOrUpdateProduct(dto);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProduct(int sellerId = 0)
        {
            try
            {
                var product = await _productService.GetProducts(sellerId);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{sellerId}/{productId}")]
        public async Task<IActionResult> DeleteProduct(int sellerId, Guid productId)
        {
            var result = await _productService.DeleteProductAsync(sellerId, productId);
            if (!result)
            {
                return NotFound();
            }
            return Ok(true);
        }
    }
}
