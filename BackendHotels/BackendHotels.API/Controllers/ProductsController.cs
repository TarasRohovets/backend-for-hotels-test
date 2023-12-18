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

        /// <summary>
        /// Get Paginated list of products
        /// </summary>
        /// <param name="filterByNameValue"></param>
        /// <param name="filterByCategoryValue"></param>
        /// <param name="orderByValue"></param>
        /// <param name="orderByDirection"></param>
        /// <returns>List of paginated products</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetProductsPaginatedAsync([FromQuery] string? filterByNameValue = null, 
            [FromQuery] string? filterByCategoryValue = null, 
            [FromQuery] string? orderByValue = null, 
            [FromQuery] string? orderByDirection = null, 
            [FromQuery] int? page = 1)
        {
            var result = await _productsService.GetProductsPaginatedAsync(filterByNameValue, filterByCategoryValue, orderByValue, orderByDirection, page);

            return Ok(result);
        }

        // REMARKS: possible endpoint for getting the details of the product when user clicks on the product line in the table
        //[HttpGet("details")]
        //public async Task<IActionResult> GetProductDetailsAsync([FromQuery] int offset, [FromQuery] int limit = 2)
        //{
        //    return Ok();
        //}
    }
}
