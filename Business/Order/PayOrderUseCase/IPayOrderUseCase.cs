namespace Business.Order.PayOrderUseCase
{
    public interface IPayOrderUseCase
    {
        Task PayOrderAsync(long id);
    }
}
