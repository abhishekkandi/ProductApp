using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Application.DTOS;
using ProductApp.Application.Interfaces;
using System.Net;
using Serilog;

namespace ProductApp.API.Controllers
{
    /// <summary>
    /// Product APIs
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly Serilog.ILogger _logger;
        private Stopwatch _stopwatch;

        /// <summary>
        /// Product Controller ctor
        /// </summary>
        /// <param name="productService"></param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
            _stopwatch = new Stopwatch();
            _logger = Log.Logger.ForContext<ProductController>();
        }
 
        /// <summary>
        /// API to get product details by its unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            _stopwatch.Start();
            var product = await _productService.GetProductByIdAsync(id);
            _stopwatch.Stop();
            _logger.Information(string.Format("Get Product By Id API | Time Elapsed: {0}ms", _stopwatch.ElapsedMilliseconds));
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// API to fetch all products by passiing PageNumber and PageSize
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedProductsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            var paginatedProducts = await _productService.GetProductsAsync(pageNumber, pageSize);
            return Ok(paginatedProducts);
        }
        
        /// <summary>
        /// API to create new product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }
 
        /// <summary>
        /// API to update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }
 
        /// <summary>
        /// API to delete a product using its unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}