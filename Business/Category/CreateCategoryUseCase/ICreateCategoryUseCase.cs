namespace Business.Category.CreateCategoryUseCase
{
    public interface ICreateCategoryUseCase
    {
        Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
    }
}
