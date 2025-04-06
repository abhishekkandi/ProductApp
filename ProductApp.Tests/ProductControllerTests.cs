using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductApp.API.Controllers;
using ProductApp.Application.Interfaces; 
using ProductApp.Application.DTOS;  

namespace ProductApp.API.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductDtoService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductDtoService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductDtoService.Object);
        }

        [Fact]
        public async Task GetProductDtoById_ReturnsNotFound_WhenProductDtoDoesNotExist()
        {
            // Arrange
            _mockProductDtoService.Setup(service => service.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetProductDtoById_ReturnsOkResult_WithProductDto()
        {
            // Arrange
            var ProductDto = new ProductDto { Id = 1, ProductName = "ProductDto1" };
            _mockProductDtoService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(ProductDto);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProductDto = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal("ProductDto1", returnProductDto.ProductName);
        }

        [Fact]
        public async Task CreateProductDto_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var ProductDto = new ProductDto { Id = 1, ProductName = "New ProductDto" };
            _mockProductDtoService.Setup(service => service.CreateProductAsync(ProductDto)).ReturnsAsync(ProductDto);

            // Act
            var result = await _controller.CreateProduct(ProductDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnProductDto = Assert.IsType<ProductDto>(createdAtActionResult.Value);
            Assert.Equal("New ProductDto", returnProductDto.ProductName);
        }

        [Fact]
        public async Task UpdateProductDto_ReturnsNoContentResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var ProductDto = new ProductDto { Id = 1, ProductName = "Updated ProductDto" };
            _mockProductDtoService.Setup(service => service.UpdateProductAsync(ProductDto.Id, ProductDto)).ReturnsAsync(ProductDto);

            // Act
            var result = await _controller.UpdateProduct(1, ProductDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProductDto = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal("Updated ProductDto", returnProductDto.ProductName);
        }

        [Fact]
        public async Task DeleteProductDto_ReturnsNoContentResult_WhenDeletionIsSuccessful()
        {
            // Arrange
            _mockProductDtoService.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnPaginatedProducts_WhenValidParameters()
        {
            // Arrange
            var paginatedProducts = new PaginatedProductsDto
            {
                Products = new List<ProductDto>
                {
                    new ProductDto { Id = 1, ProductName = "Product 1" },
                    new ProductDto { Id = 2, ProductName = "Product 2" }
                },
                TotalCount = 2
            };

            _mockProductDtoService.Setup(service => service.GetProductsAsync(1, 10))
                .ReturnsAsync(paginatedProducts);

            // Act
            var result = await _controller.GetProducts(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PaginatedProductsDto>(okResult.Value);
            Assert.Equal(2, returnValue.TotalCount);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnBadRequest_WhenInvalidParameters()
        {
            // Arrange
            _mockProductDtoService.Setup(service => service.GetProductsAsync(-1, 10))
                .ThrowsAsync(new ArgumentException("Page number and page size must be greater than zero."));

            // Act
            var result = await _controller.GetProducts(-1, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

