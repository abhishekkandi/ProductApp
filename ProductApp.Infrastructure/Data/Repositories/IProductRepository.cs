using ProductApp.Infrastructure.Data.Configurations;
 
namespace ProductApp.Infrastructure.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsAsync(int pageNumber = 1, int pageSize = 10);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}