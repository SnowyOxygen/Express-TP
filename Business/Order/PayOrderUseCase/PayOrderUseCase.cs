
using Business.Order.Exceptions;
using Infrastructure.Repositories.Interfaces;

namespace Business.Order.PayOrderUseCase
{
    public class PayOrderUseCase(IOrderRepository repository) : IPayOrderUseCase
    {
        private readonly IOrderRepository _repository = repository;

        public async Task PayOrderAsync(long id)
        {
            Model.DAO.Order order = await _repository.GetByIdAsync(id) 
                ?? throw new OrderNotFoundException(id);

            if (order.PaidTimestamp != null) throw new InvalidOperationException("Order is already paid.");
            if(order.CanceledTimestamp != null) throw new InvalidOperationException("Cannot pay a canceled order.");

            order.PaidTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            await _repository.UpdateAsync(order);
        }
    }
}
