using Infrastructure.Repositories.Interfaces;
using Model.DAO;

namespace Business.Category.CreateCategoryUseCase
{
    public class CreateCategoryUseCase(ICategoryRepository repository) : ICreateCategoryUseCase
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
        {
            // Verifications already included in CreateRequest
            // Verify that category with the same title does not already exist
            Model.DAO.Category? existing = await _repository.GetByTitleAsync(request.Title);

            if(existing != null) throw new InvalidOperationException("Category with the same title already exists.");

            Model.DAO.Category category = request.ToDAO();

            Model.DAO.Category createdCategory = await _repository.CreateOneAsync(category);

            return createdCategory.ToDto();
        }
    }
}
