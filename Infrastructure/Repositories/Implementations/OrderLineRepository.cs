using Infrastructure.DataContext;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.Repositories.Implementations
{
    public class OrderLineRepository(ExpressDataContext context) : IOrderLineRepository
    {
        private readonly ExpressDataContext _context = context;

        public async Task<IEnumerable<OrderLine>> CreateManyAsync(IEnumerable<OrderLine> orderLines)
        {
            await _context.OrderLines.AddRangeAsync(orderLines);
            await _context.SaveChangesAsync();
            return orderLines;
        }

        public async Task<IEnumerable<OrderLine>> GetByOrderIdAsync(long Id)
        {
            return await _context.OrderLines
                .Where(ol => ol.OrderId == Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderLine>> GetByProductIdAsync(long Id)
        {
            return await _context.OrderLines
                .Where(ol => ol.ProductId == Id)
                .ToListAsync();
        }
    }
}
