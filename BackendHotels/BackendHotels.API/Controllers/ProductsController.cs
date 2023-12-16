using BackendHotels.Services;
using BackendHotels.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendHotels.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetProductsPaginatedAsync([FromQuery] string? filterByNameValue = null, [FromQuery] string? filterByCategoryValue = null, [FromQuery] string? orderByValue = null, [FromQuery] string? orderByDirection = null)
        {
            var result = await _productsService.GetProductsPaginatedAsync(filterByNameValue, filterByCategoryValue, orderByValue, orderByDirection);

            return Ok(result);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetProductDetailsAsync([FromQuery] int offset, [FromQuery] int limit = 2)
        {

            return Ok();
        }
    }
}
