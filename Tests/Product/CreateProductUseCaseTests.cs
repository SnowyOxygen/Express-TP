using AutoFixture.Xunit2;
using Business.Product.CreateProductUseCase;
using Infrastructure.Repositories.Interfaces;
using Moq;

namespace Tests.Product
{
    public class CreateProductUseCaseTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

        private readonly CreateProductUseCase _sut;

        public CreateProductUseCaseTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _sut = new CreateProductUseCase(_repositoryMock.Object, _categoryRepositoryMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task CreateProduct_ShouldThrow_IfExists(CreateProductRequest request, Model.DAO.Product existingProduct)
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByTitleAsync(request.Title))
                .ReturnsAsync(existingProduct);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(request));
            _repositoryMock.Verify(r => r.GetByTitleAsync(request.Title), Times.Once);
            _repositoryMock.Verify(r => r.CreateOneAsync(It.IsAny<Model.DAO.Product>()), Times.Never);
        }

        [Theory]
        [AutoData]
        public async Task CreateProduct_ShouldThrow_IfCategoryNotExists(CreateProductRequest request)
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByTitleAsync(request.Title))
                .ReturnsAsync((Model.DAO.Product?)null);
            _categoryRepositoryMock.Setup(c => c.GetByIdAsync(request.CategoryId))
                .ReturnsAsync((Model.DAO.Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(request));
            _repositoryMock.Verify(r => r.GetByTitleAsync(request.Title), Times.Once);
            _categoryRepositoryMock.Verify(c => c.GetByIdAsync(request.CategoryId), Times.Once);
            _repositoryMock.Verify(r => r.CreateOneAsync(It.IsAny<Model.DAO.Product>()), Times.Never);
        }

        [Theory]
        [AutoData]
        public async Task CreateProduct_ShouldThrow_IfSalePriceLowerThanPrice(CreateProductRequest request, Model.DAO.Category category)
        {
            // Arrange
            request.SalePrice = request.Price + 10; // Ensure sale price is greater than price
            _repositoryMock.Setup(r => r.GetByTitleAsync(request.Title))
                .ReturnsAsync((Model.DAO.Product?) null);
            _categoryRepositoryMock.Setup(c => c.GetByIdAsync(request.CategoryId))
                .ReturnsAsync(category);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(request));
            _repositoryMock.Verify(r => r.GetByTitleAsync(request.Title), Times.Once);
            _categoryRepositoryMock.Verify(c => c.GetByIdAsync(request.CategoryId), Times.Once);
            _repositoryMock.Verify(r => r.CreateOneAsync(It.IsAny<Model.DAO.Product>()), Times.Never);
        }

        [Theory]
        [AutoData]
        public async Task CreateProduct_ShouldCreateOne(CreateProductRequest request, Model.DAO.Category category, Model.DAO.Product createdProduct)
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByTitleAsync(request.Title))
                .ReturnsAsync((Model.DAO.Product?) null);
            _categoryRepositoryMock.Setup(c => c.GetByIdAsync(request.CategoryId))
                .ReturnsAsync(category);
            _repositoryMock.Setup(r => r.CreateOneAsync(It.IsAny<Model.DAO.Product>()))
                .ReturnsAsync(createdProduct);
            request.SalePrice = request.Price - 10; // Ensure sale price is less than price

            // Act
            var result = await _sut.CreateAsync(request);

            // Assert
            _repositoryMock.Verify(r => r.GetByTitleAsync(request.Title), Times.Once);
            _categoryRepositoryMock.Verify(c => c.GetByIdAsync(request.CategoryId), Times.Once);
            _repositoryMock.Verify(r => r.CreateOneAsync(It.Is<Model.DAO.Product>(p =>
                p.Title == request.Title &&
                p.Description == request.Description &&
                p.Price == request.Price &&
                p.SalePrice == request.SalePrice &&
                p.Stock == request.Stock &&
                p.CategoryId == request.CategoryId
            )), Times.Once);
        }
    }
}
