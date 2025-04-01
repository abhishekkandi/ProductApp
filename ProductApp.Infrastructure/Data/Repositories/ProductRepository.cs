using ProductApp.Infrastructure.Data.Configurations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductApp.Infrastructure.Data;
 
namespace ProductApp.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
 
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
 
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
 
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
 
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
 
        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}