using AutoFixture.Xunit2;
using Business.Category.GetAllCategoriesUseCase;
using Infrastructure.Repositories.Interfaces;
using Moq;

namespace Tests.Category
{
    public class GetAllCategoriesUseCaseTests
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;

        private readonly GetAllCategoriesUseCase _sut;

        public GetAllCategoriesUseCaseTests()
        {
            _repositoryMock = new Mock<ICategoryRepository>();
            _sut = new GetAllCategoriesUseCase(_repositoryMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task GetAllCategories_ShouldReturnList(IEnumerable<Model.DAO.Category> categories)
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.Equal(categories.Count(), result.Count());
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
