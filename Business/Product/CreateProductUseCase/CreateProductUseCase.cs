using Infrastructure.Repositories.Interfaces;

namespace Business.Product.CreateProductUseCase
{
    public class CreateProductUseCase(
        IProductRepository repository, 
        ICategoryRepository _categoryRepository) 
        : ICreateProductUseCase
    {
        private readonly IProductRepository _repository = repository;
        private readonly ICategoryRepository _categoryRepository = _categoryRepository;

        public async Task<ProductDto> CreateAsync(CreateProductRequest request)
        {
            // Verify if product with the same title already exists
            Model.DAO.Product? existing = await _repository.GetByTitleAsync(request.Title);
            if (existing != null)
            {
                throw new InvalidOperationException("Product with the same title already exists.");
            }

            // Verify if category exists
            Model.DAO.Category category = await _categoryRepository.GetByIdAsync(request.CategoryId)
                ?? throw new InvalidOperationException($"Category with id {request.CategoryId} not found.");

            // Verify if sale price < purchase price
            if (request.SalePrice >= request.Price)
            {
                throw new InvalidOperationException("Sale price cannot be greater than purchase price.");
            }

            // Checks included in CreateRequest obj:
            // - Title between 2 and 100 characters
            // - Description between 2 and 500 characters
            // - Price between 0 and 30000
            // - SalePrice between 0 and 30000
            // - Stock greater than 0
            Model.DAO.Product product = request.ToDao();
            
            Model.DAO.Product created = await _repository.CreateOneAsync(product);

            return created.ToDto();
        }
    }
}
