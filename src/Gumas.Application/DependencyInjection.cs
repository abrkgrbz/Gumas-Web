using Gumas.Application.Mappings;
using Gumas.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gumas.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ICorporateService, CorporateService>();
        services.AddScoped<IActivityLogService, ActivityLogService>();
        services.AddScoped<ISettingService, SettingService>();

        return services;
    }
}
