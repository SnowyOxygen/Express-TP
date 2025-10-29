namespace Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Model.DAO.Order?> GetByIdAsync(long Id);
        Task<Model.DAO.Order> CreateAsync(Model.DAO.Order order);
        Task UpdateAsync(Model.DAO.Order order);
    }
}
