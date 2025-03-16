using eCommerce.BusinessLogicLayer.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.ProductService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // TO DO: Add Business Logic Layer services into the IoC container
        services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
        return services;
    }
}


