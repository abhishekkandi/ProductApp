using ProductApp.Infrastructure.Data.Configurations;
using System.Threading.Tasks;
 
namespace ProductApp.Infrastructure.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}