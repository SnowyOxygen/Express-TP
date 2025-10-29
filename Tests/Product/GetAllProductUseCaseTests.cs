using AutoFixture.Xunit2;
using Business.Product;
using Business.Product.GetAllProductUseCase;
using Infrastructure.Repositories.Interfaces;
using Moq;

namespace Tests.Product
{
    public class GetAllProductUseCaseTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;

        private readonly GetAllProductsUseCase _sut;

        public GetAllProductUseCaseTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _sut = new GetAllProductsUseCase(_repositoryMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task GetAllProduct_ShouldReturnList(List<Model.DAO.Product> products)
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            IEnumerable<ProductDto> result = await _sut.GetAllAsync();

            // Assert
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.Equal(products.Count, result.Count());
            foreach (var product in products)
            {
                Assert.Contains(result, dto => dto.Id == product.Id && dto.Title == product.Title);
            }
        }
    }
}
