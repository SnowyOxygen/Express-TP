namespace Business.Product.CreateProductUseCase
{
    public interface ICreateProductUseCase
    {
        Task<ProductDto> CreateAsync(CreateProductRequest request);
    }
}
