using Business.Category.CreateCategoryUseCase;
using Business.Category.GetAllCategoriesUseCase;
using Business.Category.GetCategoryByIdUseCase;
using Infrastructure.Repositories.Implementations;
using Infrastructure.Repositories.Interfaces;

namespace API.DependancyInjection
{
    internal static class CategoryInjections
    {
        internal static void AddCategoryMappings(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();
            services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
            services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        }
    }
}
