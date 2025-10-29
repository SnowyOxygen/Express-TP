namespace Business.Category.DeleteCategoryUseCase
{
    public interface IDeleteCategoryUseCase
    {
        Task DeleteCategoryAsync(long categoryId);
    }
}
