using Model.DAO;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(long id);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<long> ids);
        Task<Product?> GetByTitleAsync(string title);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> CreateOneAsync(Product product);
    }
}
