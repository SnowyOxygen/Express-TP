namespace Infrastructure.Repositories.Interfaces
{
    public interface IOrderLineRepository
    {
        Task<IEnumerable<Model.DAO.OrderLine>> GetByOrderIdAsync(long Id);
        Task<IEnumerable<Model.DAO.OrderLine>> GetByProductIdAsync(long Id);
        Task<IEnumerable<Model.DAO.OrderLine>> CreateManyAsync(IEnumerable<Model.DAO.OrderLine> orderLines);
    }
}
