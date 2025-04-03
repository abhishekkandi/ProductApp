using ProductApp.Infrastructure.Data.Repositories;

namespace ProductApp.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        Task<int> CompleteAsync();
    }
}
