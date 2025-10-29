namespace Business.Category.GetAllCategoriesUseCase
{
    public interface IGetAllCategoriesUseCase
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
    }
}
