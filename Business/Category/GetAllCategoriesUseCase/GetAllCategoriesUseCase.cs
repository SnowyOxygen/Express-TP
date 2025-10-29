using Infrastructure.Repositories.Interfaces;

namespace Business.Category.GetAllCategoriesUseCase
{
    public class GetAllCategoriesUseCase(ICategoryRepository repository) : IGetAllCategoriesUseCase
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            IEnumerable<Model.DAO.Category> categories = await _repository.GetAllAsync();

            return categories.Select(c => c.ToDto());
        }
    }
}
