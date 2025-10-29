using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DataContext;

public class UnitOfWork : IUnitOfWork
{
    private readonly ExpressDataContext _context;
    private IDbContextTransaction? _transaction;
    
    public IOrderRepository Orders { get; }
    public IOrderLineRepository OrderLines { get; }
    public IProductRepository Products { get; }

    public UnitOfWork(
        ExpressDataContext context,
        IOrderRepository orderRepository,
        IOrderLineRepository orderLineRepository,
        IProductRepository productRepository)
    {
        _context = context;
        Orders = orderRepository;
        OrderLines = orderLineRepository;
        Products = productRepository;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}