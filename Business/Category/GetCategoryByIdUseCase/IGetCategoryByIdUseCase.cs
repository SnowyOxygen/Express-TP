namespace Business.Category.GetCategoryByIdUseCase
{
    public interface IGetCategoryByIdUseCase
    {
        Task<CategoryDto?> GetByIdAsync(long id);
    }
}
