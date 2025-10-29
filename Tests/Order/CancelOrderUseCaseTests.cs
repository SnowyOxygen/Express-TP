using AutoFixture;
using AutoFixture.Xunit2;
using Business.Order.CancelOrderUseCase;
using Business.Order.Exceptions;
using Infrastructure.Repositories.Interfaces;
using Moq;

namespace Tests.Order
{
    public class CancelOrderUseCaseTests
    {
        private readonly Mock<IOrderRepository> _mockRepository;
        private readonly ICancelOrderUseCase _useCase;
        private readonly IFixture _fixture;

        public CancelOrderUseCaseTests()
        {
            _mockRepository = new Mock<IOrderRepository>();
            _useCase = new CancelOrderUseCase(_mockRepository.Object);
            
            // Configure AutoFixture to handle circular references
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory]
        [AutoData]
        public async Task CancelOrderAsync_WithValidUncanceledOrder_ShouldUpdateOrderWithCancelTimestamp(
            long orderId)
        {
            // Arrange
            var order = _fixture.Build<Model.DAO.Order>()
                .With(o => o.Id, orderId)
                .With(o => o.PaidTimestamp, (long?)null)
                .With(o => o.CanceledTimestamp, (long?)null)
                .Create();

            _mockRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Model.DAO.Order>())).Returns(Task.CompletedTask);

            // Act
            await _useCase.CancelOrderAsync(orderId);

            // Assert
            Assert.NotNull(order.CanceledTimestamp);
            _mockRepository.Verify(r => r.UpdateAsync(order), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task CancelOrderAsync_WithNonExistentOrder_ShouldThrowOrderNotFoundException(
            long orderId)
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Model.DAO.Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<OrderNotFoundException>(() => 
                _useCase.CancelOrderAsync(orderId));
        }

        [Theory]
        [AutoData]
        public async Task CancelOrderAsync_WithAlreadyCanceledOrder_ShouldThrowInvalidOperationException(
            long orderId)
        {
            // Arrange
            var order = _fixture.Build<Model.DAO.Order>()
                .With(o => o.Id, orderId)
                .With(o => o.CanceledTimestamp, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .Create();

            _mockRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.CancelOrderAsync(orderId));
            Assert.Equal("Order is already canceled.", exception.Message);
        }

        [Theory]
        [AutoData]
        public async Task CancelOrderAsync_WithPaidOrder_ShouldThrowInvalidOperationException(
            long orderId)
        {
            // Arrange
            var order = _fixture.Build<Model.DAO.Order>()
                .With(o => o.Id, orderId)
                .With(o => o.PaidTimestamp, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .With(o => o.CanceledTimestamp, (long?)null)
                .Create();

            _mockRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.CancelOrderAsync(orderId));
            Assert.Equal("Cannot cancel a paid order.", exception.Message);
        }
    }
}
