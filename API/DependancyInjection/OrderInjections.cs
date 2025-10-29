using Business.Order.CancelOrderUseCase;
using Business.Order.CreateOrderUseCase;
using Business.Order.GetOrderByIdUseCase;
using Business.Order.PayOrderUseCase;
using Infrastructure.Repositories.Implementations;
using Infrastructure.Repositories.Interfaces;

namespace API.DependancyInjection
{
    internal static class OrderInjections
    {
        internal static void AddOrderMappings(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderLineRepository, OrderLineRepository>();

            services.AddScoped<IGetOrderByIdUseCase, GetOrderByIdUseCase>();
            services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
            services.AddScoped<ICancelOrderUseCase, CancelOrderUseCase>();
            services.AddScoped<IPayOrderUseCase, PayOrderUseCase>();
        }
    }
}
