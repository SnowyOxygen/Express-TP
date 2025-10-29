using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DataContext;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.Repositories.Implementations
{
    public class ProductRepository(ExpressDataContext context) : IProductRepository
    {
        private readonly ExpressDataContext _context = context;

        public async Task<Product> CreateOneAsync(Product product)
        {
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetByIdAsync(long id)
        {
            return await _context.FindAsync<Product>(id);
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<long> ids)
        {
            return await _context.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<Product?> GetByTitleAsync(string title)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Title == title);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}
