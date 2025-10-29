using Infrastructure.DataContext;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.Repositories.Implementations
{
    public class CategoryRepository(ExpressDataContext context) : ICategoryRepository
    {
        private readonly ExpressDataContext _context = context;

        public async Task<Category> CreateOneAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> GetByIdAsync(long id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category?> GetByTitleAsync(string title)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Title == title);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
