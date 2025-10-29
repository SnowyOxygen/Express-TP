using AutoFixture;
using AutoFixture.Xunit2;
using Business.Order.CreateOrderUseCase;
using Infrastructure.DataContext;
using Infrastructure.Repositories.Interfaces;
using Model.DAO;
using Moq;

namespace Tests.Order
{
    public class CreateOrderUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderLineRepository> _mockOrderLineRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ICreateOrderUseCase _useCase;
        private readonly IFixture _fixture;

        public CreateOrderUseCaseTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderLineRepository = new Mock<IOrderLineRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            // Setup UnitOfWork to return our repository mocks
            _mockUnitOfWork.Setup(uow => uow.Orders).Returns(_mockOrderRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.OrderLines).Returns(_mockOrderLineRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepository.Object);

            _useCase = new CreateOrderUseCase(_mockUnitOfWork.Object);

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory]
        [AutoData]
        public async Task CreateAsync_WithValidOrderRequest_ShouldCreateOrderAndOrderLines(long orderId)
        {
            // Arrange
            var product = _fixture.Build<Model.DAO.Product>()
                .With(p => p.Stock, 100)
                .With(p => p.Price, 1000)
                .With(p => p.SalePrice, 800)
                .With(p => p.SaleActive, false)
                .Create();

            var request = new CreateOrderRequest
            {
                OrderLines = new[]
                {
                    new CreateOrderLine { ProductId = product.Id, Quantity = 2 }
                }
            };

            _mockOrderRepository
                .Setup(r => r.CreateAsync(It.IsAny<Model.DAO.Order>()))
                .ReturnsAsync((Model.DAO.Order o) => { o.Id = orderId; return o; });

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(new[] { product });

            _mockOrderLineRepository
                .Setup(r => r.GetByProductIdAsync(product.Id))
                .ReturnsAsync(Array.Empty<Model.DAO.OrderLine>());

            // Act
            long createdOrderId = await _useCase.CreateAsync(request);

            // Assert
            Assert.Equal(orderId, createdOrderId);
            _mockOrderRepository.Verify(r => r.CreateAsync(It.IsAny<Model.DAO.Order>()), Times.Once);
            _mockOrderLineRepository.Verify(r => r.CreateManyAsync(It.Is<IEnumerable<Model.DAO.OrderLine>>(
                ol => ol.First().UnitPrice == 1000)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task CreateAsync_WithTooManyProducts_ShouldThrowInvalidOperationException(long dummyParam)
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                OrderLines = Enumerable.Range(1, 6).Select(i => new CreateOrderLine 
                { 
                    ProductId = i, 
                    Quantity = 1 
                })
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.CreateAsync(request));
            Assert.Equal("Too many products selected.", exception.Message);
            _mockUnitOfWork.Verify(uow => uow.BeginTransactionAsync(), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Theory]
        [AutoData]
        public async Task CreateAsync_WithNonExistentProduct_ShouldThrowInvalidOperationException(long productId)
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                OrderLines = new[]
                {
                    new CreateOrderLine { ProductId = productId, Quantity = 1 }
                }
            };

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(Array.Empty<Model.DAO.Product>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.CreateAsync(request));
            Assert.Equal($"Product with id {productId} not found.", exception.Message);
            _mockUnitOfWork.Verify(uow => uow.RollbackAsync(), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task CreateAsync_WithInsufficientStock_ShouldThrowInvalidOperationException(long dummyParam)
        {
            // Arrange
            var product = _fixture.Build<Model.DAO.Product>()
                .With(p => p.Stock, 5)
                .Create();

            var existingOrderLines = new[]
            {
                new OrderLine
                {
                    OrderId = 0,
                    ProductId = product.Id,
                    UnitPrice = product.Price,
                    Quantity = 3
                }
            };

            var request = new CreateOrderRequest
            {
                OrderLines = new[]
                {
                    new CreateOrderLine { ProductId = product.Id, Quantity = 3 }
                }
            };

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(new[] { product });

            _mockOrderLineRepository
                .Setup(r => r.GetByProductIdAsync(product.Id))
                .ReturnsAsync(existingOrderLines);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.CreateAsync(request));
            Assert.Equal($"Insufficient stock for product id {product.Id}. Requested: 3, Available: 2", 
                exception.Message);
            _mockUnitOfWork.Verify(uow => uow.RollbackAsync(), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task CreateAsync_WithProductOnSale_ShouldUseSalePrice(long orderId)
        {
            // Arrange
            var product = _fixture.Build<Model.DAO.Product>()
                .With(p => p.Stock, 10)
                .With(p => p.Price, 1000)
                .With(p => p.SalePrice, 800)
                .With(p => p.SaleActive, true)
                .Create();

            var request = new CreateOrderRequest
            {
                OrderLines = new[]
                {
                    new CreateOrderLine { ProductId = product.Id, Quantity = 1 }
                }
            };

            _mockOrderRepository
                .Setup(r => r.CreateAsync(It.IsAny<Model.DAO.Order>()))
                .ReturnsAsync((Model.DAO.Order o) => { o.Id = orderId; return o; });

            _mockProductRepository
                .Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(new[] { product });

            _mockOrderLineRepository
                .Setup(r => r.GetByProductIdAsync(product.Id))
                .ReturnsAsync(Array.Empty<Model.DAO.OrderLine>());

            // Act
            await _useCase.CreateAsync(request);

            // Assert
            _mockOrderLineRepository.Verify(r => r.CreateManyAsync(It.Is<IEnumerable<Model.DAO.OrderLine>>(
                ol => ol.First().UnitPrice == 800)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}
