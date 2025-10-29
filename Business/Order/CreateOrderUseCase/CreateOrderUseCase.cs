using Infrastructure.Repositories.Interfaces;

namespace Business.Order.CreateOrderUseCase
{
    public class CreateOrderUseCase(
        IOrderRepository repository,
        IOrderLineRepository orderLineRepository,
        IProductRepository productRepository) : ICreateOrderUseCase
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IOrderLineRepository _orderLineRepository = orderLineRepository;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<long> CreateAsync(CreateOrderRequest request)
        {
            // TODO: implement unit of work in case of failure during order line creation
            if (request.OrderLines.Count() > 5) throw new InvalidOperationException("Too many products selected.");

            Model.DAO.Order order = request.ToDao();
            order = await _repository.CreateAsync(order);

            await CreateOrderLinesAsync(request, order);

            return order.Id;
        }

        private async Task CreateOrderLinesAsync(CreateOrderRequest request, Model.DAO.Order order)
        {
            // Validate order lines
            IEnumerable<long> productIds = request.OrderLines.Select(ol => ol.ProductId);
            IEnumerable<Model.DAO.Product> products = await _productRepository.GetByIdsAsync(productIds);
            IEnumerable<Model.DAO.OrderLine> orderLines = [];

            foreach (long productId in productIds)
            {
                // Check if product exists
                Model.DAO.Product? product = products.FirstOrDefault(p => p.Id == productId);
                CreateOrderLine createOrderLine = request.OrderLines.First(ol => ol.ProductId == productId);

                await ValidateOrderLine(productId, product, createOrderLine);

                // Create order line and add to list
                int unitPrice = product.SaleActive ? product.SalePrice : product.Price;
                Model.DAO.OrderLine orderLine = createOrderLine.ToDao(order.Id, unitPrice);
                orderLines = orderLines.Append(orderLine);
            }

            await _orderLineRepository.CreateManyAsync(orderLines);
        }

        private async Task ValidateOrderLine(long productId, Model.DAO.Product? product, CreateOrderLine createOrderLine)
        {
            if (product == null)
            {
                throw new InvalidOperationException($"Product with id {productId} not found.");
            }

            // Check if stock is sufficient
            int currentStock = await CalculateProductStockAsync(product);
            if (currentStock - createOrderLine.Quantity < 0)
            {
                throw new InvalidOperationException($"Insufficient stock for product id {productId}. Requested: {createOrderLine.Quantity}, Available: {currentStock}");
            }
        }

        private async Task<int> CalculateProductStockAsync(Model.DAO.Product product)
        {
            IEnumerable<Model.DAO.OrderLine> orderLines = await _orderLineRepository.GetByProductIdAsync(product.Id);
            int usedStock = orderLines.Sum(ol => ol.Quantity);
            int stockLeft = product.Stock - usedStock;

            return stockLeft;
        }
    }
}
