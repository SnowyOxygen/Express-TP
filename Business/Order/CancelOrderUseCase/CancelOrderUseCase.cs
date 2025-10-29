using Business.Order.Exceptions;
using Infrastructure.Repositories.Interfaces;

namespace Business.Order.CancelOrderUseCase
{
    public class CancelOrderUseCase(IOrderRepository repository) : ICancelOrderUseCase
    {
        private readonly IOrderRepository _repository = repository;

        public async Task CancelOrderAsync(long id)
        {
            Model.DAO.Order order = await _repository.GetByIdAsync(id) 
                ?? throw new OrderNotFoundException(id);

            if (order.CanceledTimestamp != null) throw new InvalidOperationException("Order is already canceled.");
            if (order.PaidTimestamp != null) throw new InvalidOperationException("Cannot cancel a paid order.");

            order.CanceledTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            await _repository.UpdateAsync(order);
        }
    }
}
