using Infrastructure.Repositories.Interfaces;

namespace Business.Order.GetOrderByIdUseCase
{
    public class GetOrderByIdUseCase(
        IOrderRepository repository,
        IProductRepository productRepository): IGetOrderByIdUseCase
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<OrderDto?> GetByIdAsync(long id)
        {
            Model.DAO.Order? order = await _repository.GetByIdAsync(id);
            if (order == null) return null;
            IEnumerable<long> productIds = order.OrderLines?.Select(ol => ol.ProductId) ?? Enumerable.Empty<long>();

            List<Model.DAO.Product> products = (await _productRepository.GetByIdsAsync(productIds)).ToList();

            return order.ToDto(products);
        }
    }
}
