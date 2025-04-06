using ProductApp.Application.DTOS;
using System.Threading.Tasks;

namespace ProductApp.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<PaginatedProductsDto> GetProductsAsync(int pageNumber = 1, int pageSize = 10);
    }
}