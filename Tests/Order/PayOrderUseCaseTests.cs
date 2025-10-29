using AutoFixture;
using AutoFixture.Xunit2;
using Business.Order.Exceptions;
using Business.Order.PayOrderUseCase;
using Infrastructure.Repositories.Interfaces;
using Moq;

namespace Tests.Order
{
    public class PayOrderUseCaseTests
    {
        private readonly Mock<IOrderRepository> _mockRepository;
        private readonly IPayOrderUseCase _useCase;
        private readonly IFixture _fixture;

        public PayOrderUseCaseTests()
        {
            _mockRepository = new Mock<IOrderRepository>();
            _useCase = new PayOrderUseCase(_mockRepository.Object);
            
            // Configure AutoFixture to handle circular references
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Theory]
        [AutoData]
        public async Task PayOrderAsync_WithValidUnpaidOrder_ShouldUpdateOrderWithPaymentTimestamp(
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
            await _useCase.PayOrderAsync(orderId);

            // Assert
            Assert.NotNull(order.PaidTimestamp);
            _mockRepository.Verify(r => r.UpdateAsync(order), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task PayOrderAsync_WithNonExistentOrder_ShouldThrowOrderNotFoundException(
            long orderId)
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Model.DAO.Order?)null);

            // Act & Assert
            await Assert.ThrowsAsync<OrderNotFoundException>(() => 
                _useCase.PayOrderAsync(orderId));
        }

        [Theory]
        [AutoData]
        public async Task PayOrderAsync_WithAlreadyPaidOrder_ShouldThrowInvalidOperationException(
            long orderId)
        {
            // Arrange
            var order = _fixture.Build<Model.DAO.Order>()
                .With(o => o.Id, orderId)
                .With(o => o.PaidTimestamp, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .Create();

            _mockRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.PayOrderAsync(orderId));
            Assert.Equal("Order is already paid.", exception.Message);
        }

        [Theory]
        [AutoData]
        public async Task PayOrderAsync_WithCanceledOrder_ShouldThrowInvalidOperationException(
            long orderId)
        {
            // Arrange
            var order = _fixture.Build<Model.DAO.Order>()
                .With(o => o.Id, orderId)
                .With(o => o.PaidTimestamp, (long?) null)
                .With(o => o.CanceledTimestamp, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .Create();

            _mockRepository.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.PayOrderAsync(orderId));
            Assert.Equal("Cannot pay a canceled order.", exception.Message);
        }
    }
}
