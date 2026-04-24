using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Options;

namespace Gumas.Web.Infrastructure;

public static class LocalizationConfig
{
    public static readonly string[] SupportedCultures = { "tr", "en" };
    public const string DefaultCulture = "tr";
    public const string CookieName = "GumasLanguage";

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = SupportedCultures
                .Select(c => new CultureInfo(c))
                .ToList();

            options.DefaultRequestCulture = new RequestCulture(DefaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            // Priority: Route -> Cookie -> Accept-Language Header
            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new RouteDataRequestCultureProvider { Options = options },
                new CookieRequestCultureProvider { CookieName = CookieName },
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });

        services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();
    }

    public static void ConfigureApp(IApplicationBuilder app)
    {
        var locOptions = app.ApplicationServices
            .GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(locOptions.Value);
    }
}
