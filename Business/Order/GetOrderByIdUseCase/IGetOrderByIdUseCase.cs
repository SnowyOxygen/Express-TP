namespace Business.Order.GetOrderByIdUseCase
{
    public interface IGetOrderByIdUseCase
    {
        Task<OrderDto?> GetByIdAsync(long id);
    }
}
