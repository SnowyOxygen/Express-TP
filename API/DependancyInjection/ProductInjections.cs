using Business.Product.CreateProductUseCase;
using Business.Product.GetAllProductUseCase;
using Infrastructure.Repositories.Implementations;
using Infrastructure.Repositories.Interfaces;

namespace API.DependancyInjection
{
    internal static class ProductInjections
    {
        internal static void AddProductMappings(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
            services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
        }
    }
}
