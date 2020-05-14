using ConsoleShopper.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleShopper.UI
{
    
    
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// // Creates an extention method for IServiceCollection, keyword 'this' in the parameter makes it an extention 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryLayerServices(this IServiceCollection services)
        {  
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}