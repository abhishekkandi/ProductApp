using Moq;
using AutoMapper;
using ProductApp.Application.DTOS;
using ProductApp.Application.Services;
using ProductApp.Infrastructure.Data.Configurations;
using ProductApp.Infrastructure.Data;
using ProductApp.Infrastructure.Data.Repositories;

namespace ProductApp.Application.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDto>().ReverseMap());
            _mapper = config.CreateMapper();
            _unitOfWorkMock.Setup(uow => uow.Products).Returns(_productRepositoryMock.Object);
            _productService = new ProductService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, ProductName = "Test Product", CreatedBy = "Admin", CreatedOn = DateTime.UtcNow };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.ProductName);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldAddProduct()
        {
            // Arrange
            var productDto = new ProductDto { ProductName = "New Product" };
            var product = new Product { ProductName = "Test Product", CreatedBy = "Admin", CreatedOn = DateTime.UtcNow };
            _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _productService.CreateProductAsync(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.ProductName, result.ProductName);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto { ProductName = "Updated Product" };
            var product = new Product { ProductName = "Test Product", CreatedBy = "Admin", CreatedOn = DateTime.UtcNow };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _productService.UpdateProductAsync(productId, productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.ProductName, result.ProductName);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductIsDeleted()
        {
            // Arrange
            var productId = 1;
            var product = new Product { ProductName = "Test Product", CreatedBy = "Admin", CreatedOn = DateTime.UtcNow };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _productService.DeleteProductAsync(productId);

            // Assert
            Assert.True(result);
        }
    }
}
