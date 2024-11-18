using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        //Add services to the container.

        //Api Endpoints Services

        //Application Use Case Services
        services.AddMediatR(config => { config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

        //Data - Infrastructure Services
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();
            options.AddInterceptors(interceptors.ToArray<IInterceptor>());
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        //Use Api Endpoints Services

        //Use Application Use Case Services

        //Use Data - Infrastructure Services

        app.UseMigration<CatalogDbContext>();

        return app;
    }
}