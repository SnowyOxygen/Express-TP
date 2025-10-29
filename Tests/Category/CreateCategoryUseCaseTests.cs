using AutoFixture.Xunit2;
using Business.Category.CreateCategoryUseCase;
using Infrastructure.Repositories.Interfaces;
using Moq;

namespace Tests.Category
{
    public class CreateCategoryUseCaseTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;

        private readonly CreateCategoryUseCase _sut;

        public CreateCategoryUseCaseTests()
        {
            _repositoryMock = new();

            _sut = new CreateCategoryUseCase(_repositoryMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task CreateCategory_ShouldThrow_IfExisting(
            CreateCategoryRequest request, 
            Model.DAO.Category existing)
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByTitleAsync(request.Title))
                .ReturnsAsync(existing);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(request));

            _repositoryMock.Verify(x => x.GetByTitleAsync(request.Title), Times.Once);
            _repositoryMock.Verify(x => x.CreateOneAsync(It.IsAny<Model.DAO.Category>()), Times.Never);
        }

        [Theory]
        [AutoData]
        public async Task CreateCategory_ShouldCreateOne_IfNotExisting(
            CreateCategoryRequest request)
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetByTitleAsync(request.Title))
                .ReturnsAsync((Model.DAO.Category?)null);
            _repositoryMock.Setup(x => x.CreateOneAsync(It.IsAny<Model.DAO.Category>()))
                .ReturnsAsync((Model.DAO.Category cat) => cat);

            // Act
            var result = await _sut.CreateAsync(request);

            // Assert
            _repositoryMock.Verify(x => x.GetByTitleAsync(request.Title), Times.Once);
            _repositoryMock.Verify(x => x.CreateOneAsync(It.Is<Model.DAO.Category>(cat => 
                cat.Title == request.Title && 
                cat.Description == request.Description)), Times.Once);
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(request.Description, result.Description);
            Assert.Equal(request.Color, result.Color);
        }
    }
}
