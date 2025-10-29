using Model.DAO;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(long id);
        Task<Category?> GetByTitleAsync(string title);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> CreateOneAsync(Category category);
    }
}
