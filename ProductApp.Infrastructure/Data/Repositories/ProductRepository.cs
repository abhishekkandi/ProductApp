using ProductApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
 
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

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            var totalCount = await _context.Products.CountAsync();
            var products = await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (products, totalCount);                
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