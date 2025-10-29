namespace Business.Order.CancelOrderUseCase
{
    public interface ICancelOrderUseCase
    {
        Task CancelOrderAsync(long id);
    }
}
