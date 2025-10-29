using Infrastructure.Repositories.Interfaces;

namespace Business.Category.GetCategoryByIdUseCase
{
    public class GetCategoryByIdUseCase(ICategoryRepository repository) : IGetCategoryByIdUseCase
    {
        private readonly ICategoryRepository repository = repository;

        public async Task<CategoryDto?> GetByIdAsync(long id)
        {
            Model.DAO.Category? category = await repository.GetByIdAsync(id);

            return category?.ToDto();
        }
    }
}
