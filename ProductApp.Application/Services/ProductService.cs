using ProductApp.Application.DTOS;
using ProductApp.Application.Interfaces;
using ProductApp.Infrastructure.Data.Configurations;
using ProductApp.Infrastructure.Data.Repositories;
using System.Threading.Tasks;
 
namespace ProductApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
 
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
 
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;
 
            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn,
                ModifiedBy = product.ModifiedBy,
                ModifiedOn = product.ModifiedOn
            };
        }
 
        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                CreatedBy = productDto.CreatedBy,
                CreatedOn = productDto.CreatedOn
            };
 
            await _productRepository.AddAsync(product);
            return productDto;
        }
 
        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;
 
            product.ProductName = productDto.ProductName;
            product.ModifiedBy = productDto.ModifiedBy;
            product.ModifiedOn = productDto.ModifiedOn;
 
            await _productRepository.UpdateAsync(product);
            return productDto;
        }
 
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;
 
            await _productRepository.DeleteAsync(product);
            return true;
        }
    }
}