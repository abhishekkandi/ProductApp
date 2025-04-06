using Microsoft.EntityFrameworkCore;
using ProductApp.Infrastructure.Data.Configurations;
using ProductApp.Infrastructure.Data;
using ProductApp.Infrastructure.Data.Repositories;

namespace ProductApp.Infrastructure.Tests
{
    public class ProductRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, ProductName = "Test Product", CreatedBy = "Admin" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddProduct_ShouldAddProductAsync()
        {
            // Arrange
            var product = new Product { Id = 2, ProductName = "New Product", CreatedBy = "Admin" };

            // Act
            await _repository.AddAsync(product);

            // Assert
            var addedProduct = await _context.Products.FindAsync(product.Id);
            Assert.NotNull(addedProduct);
            Assert.Equal("New Product", addedProduct.ProductName);
        }

        [Fact]
        public async Task UpdateProduct_ShouldUpdateProductAsync()
        {
            // Arrange
            var product = new Product { Id = 3, ProductName = "Old Product", CreatedBy = "Admin" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            product.ProductName = "Updated Product";

            // Act
            await _repository.UpdateAsync(product);

            // Assert
            var updatedProduct = await _context.Products.FindAsync(product.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.ProductName);
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProductAsync()
        {
            // Arrange
            var product = new Product { Id = 4, ProductName = "Product to Delete", CreatedBy = "Admin" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(product);
            
            // Assert
            var deletedProduct = await _context.Products.FindAsync(product.Id);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnPaginatedProducts_WhenValidParameters()
        {
            // Arrange
            var product = new Product { Id = 11, ProductName = "Test Product", CreatedBy = "Admin" };
            var product2 = new Product { Id = 12, ProductName = "Test Product2", CreatedBy = "Admin" };
            var product3 = new Product { Id = 13, ProductName = "Test Product3", CreatedBy = "Admin" };
            await _context.Products.AddRangeAsync(new List<Product> { product, product2, product3 });
            await _context.SaveChangesAsync();

            //Act
            var (products, totalCount) = await _repository.GetProductsAsync(1, 2);

            // Assert
            Assert.NotEmpty(products);
        }

    }
}
