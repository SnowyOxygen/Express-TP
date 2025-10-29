using Infrastructure.DataContext;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.Repositories.Implementations
{
    public class OrderRepository(ExpressDataContext context) : IOrderRepository
    {
        private readonly ExpressDataContext _context = context;

        public async Task<Order?> GetByIdAsync(long Id)
        {
            return await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.Id == Id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
