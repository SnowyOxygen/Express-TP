using Infrastructure.DataContext;
using Infrastructure.Repositories.Interfaces;

namespace Business.Order.CreateOrderUseCase
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderLineRepository _orderLineRepository;
        private readonly IProductRepository _productRepository;

        public CreateOrderUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = unitOfWork.Orders;
            _orderLineRepository = unitOfWork.OrderLines;
            _productRepository = unitOfWork.Products;
        }

        public async Task<long> CreateAsync(CreateOrderRequest request)
        {
            if (request.OrderLines.Count() > 5) throw new InvalidOperationException("Too many products selected.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                Model.DAO.Order order = request.ToDao();
                order = await _orderRepository.CreateAsync(order);

                await CreateOrderLinesAsync(request, order);
                
                await _unitOfWork.CommitAsync();

                return order.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                throw;
            }
        }

        private async Task CreateOrderLinesAsync(CreateOrderRequest request, Model.DAO.Order order)
        {
            IEnumerable<long> productIds = request.OrderLines.Select(ol => ol.ProductId);
            IEnumerable<Model.DAO.Product> products = await _productRepository.GetByIdsAsync(productIds);
            IEnumerable<Model.DAO.OrderLine> orderLines = [];

            foreach (long productId in productIds)
            {
                Model.DAO.Product? product = products.FirstOrDefault(p => p.Id == productId);
                CreateOrderLine createOrderLine = request.OrderLines.First(ol => ol.ProductId == productId);

                await ValidateOrderLine(productId, product, createOrderLine);

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
