using ConsoleShopper.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;



namespace ConsoleShopper.UI
{
    public static class ContainerBuilder
    {
        /// <summary>
        /// Fills in and Builds Dependency Injection container. 
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider Build()
        {
            // Create a list of dependencies
            // You need to install Microsoft.Extensions.DependencyInjection through nuget to be able to use it.
            var services = new ServiceCollection();


            // Build configuration to access appsettings.json file 
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = configurationBuilder.Build();

            // remove this line, this is for debugging only. 
            var test = configuration.GetSection("Logging");


            // from this point onwards dependencies can be added to the DI container via service collection. 
            // service collection is the bucket/container that holds all the dependencies we inject here. 

        

            // Adding DbContext into DI Container.
            services.AddDbContext<ConsoleShopperDbContext>(options => options
                // Use DefaultConnection for passworded sa connection 
                // Use AlternativeConnection for windows authenticated connection
                .UseSqlServer(configuration.GetConnectionString("AlternateConnection"),
                        options => options.MigrationsAssembly("ConsoleShopper.Repository")));

            // Adding Repository Layer dependencies into DI Container
            services.AddRepositoryLayerServices();

            // Adding Configuration into DI Container
            services.AddSingleton<IConfiguration>(configuration);
            
            // Here we are injecting Logging service into it. 

            services.AddLogging((configure) => 
            {
                configure.ClearProviders();
                configure.AddConfiguration(configuration.GetSection("Logging"));
                configure.AddConsole(c =>
                {
                    c.TimestampFormat = "[HH:mm:ss] ";
                });
                configure.SetMinimumLevel(LogLevel.Trace);
            });
            return services.BuildServiceProvider();
        }

    }
}
