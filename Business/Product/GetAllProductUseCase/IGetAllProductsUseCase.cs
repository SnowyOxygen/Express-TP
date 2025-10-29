namespace Business.Product.GetAllProductUseCase
{
    public interface IGetAllProductsUseCase
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
    }
}
