using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using Gumas.Application;
using Gumas.Infrastructure;
using Gumas.Infrastructure.Data;
using Gumas.Infrastructure.Data.Seed;
using Gumas.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add localization services
LocalizationConfig.ConfigureServices(builder.Services);

// Add services to the container
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks();

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/javascript",
        "application/json",
        "text/css",
        "text/html",
        "text/xml",
        "text/plain",
        "image/svg+xml"
    });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

// Response Caching
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();

// Configure ForwardedHeaders for reverse proxy (Traefik/Coolify)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Admin Authentication
builder.Services.AddAuthentication("AdminCookie")
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/admin/giris";
        options.LogoutPath = "/admin/cikis";
        options.AccessDeniedPath = "/admin/giris";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// Add route constraint for language
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// ForwardedHeaders must be before any other middleware that depends on scheme
app.UseForwardedHeaders();

// Response compression (should be early in the pipeline)
app.UseResponseCompression();

// Static files with caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 year in production
        if (!app.Environment.IsDevelopment())
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000,immutable");
        }
    }
});

app.UseRouting();

// Response caching
app.UseResponseCaching();

// Add localization middleware (BEFORE authentication)
LocalizationConfig.ConfigureApp(app);

app.UseAuthentication();
app.UseAuthorization();

// Seed database
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<GumasDbContext>();
    context.Database.EnsureCreated();
    await DataSeeder.SeedAsync(context);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding the database.");
}

// ========================================
// ENGLISH ROUTES (with /en prefix)
// ========================================
app.MapControllerRoute(
    name: "en-home",
    pattern: "en",
    defaults: new { controller = "Home", action = "Index", culture = "en" });

app.MapControllerRoute(
    name: "en-products",
    pattern: "en/products",
    defaults: new { controller = "Products", action = "Index", culture = "en" });

app.MapControllerRoute(
    name: "en-product-detail",
    pattern: "en/product/{slug}",
    defaults: new { controller = "Products", action = "Detail", culture = "en" });

app.MapControllerRoute(
    name: "en-brands",
    pattern: "en/brands",
    defaults: new { controller = "Brands", action = "Index", culture = "en" });

app.MapControllerRoute(
    name: "en-brand-detail",
    pattern: "en/brand/{slug}",
    defaults: new { controller = "Brands", action = "Detail", culture = "en" });

app.MapControllerRoute(
    name: "en-ecatalogue",
    pattern: "en/e-catalogue",
    defaults: new { controller = "Brands", action = "ECatalogue", culture = "en" });

app.MapControllerRoute(
    name: "en-news",
    pattern: "en/news",
    defaults: new { controller = "News", action = "Index", culture = "en" });

app.MapControllerRoute(
    name: "en-news-detail",
    pattern: "en/news/{slug}",
    defaults: new { controller = "News", action = "Detail", culture = "en" });

app.MapControllerRoute(
    name: "en-about",
    pattern: "en/about",
    defaults: new { controller = "Home", action = "About", culture = "en" });

app.MapControllerRoute(
    name: "en-contact",
    pattern: "en/contact",
    defaults: new { controller = "Contact", action = "Index", culture = "en" });

// ========================================
// TURKISH ROUTES (default, no prefix)
// ========================================
app.MapControllerRoute(
    name: "products",
    pattern: "urunler",
    defaults: new { controller = "Products", action = "Index", culture = "tr" });

app.MapControllerRoute(
    name: "product-detail",
    pattern: "urun/{slug}",
    defaults: new { controller = "Products", action = "Detail", culture = "tr" });

app.MapControllerRoute(
    name: "brands",
    pattern: "markalar",
    defaults: new { controller = "Brands", action = "Index", culture = "tr" });

app.MapControllerRoute(
    name: "brand-detail",
    pattern: "marka/{slug}",
    defaults: new { controller = "Brands", action = "Detail", culture = "tr" });

app.MapControllerRoute(
    name: "ecatalogue",
    pattern: "e-katalog",
    defaults: new { controller = "Brands", action = "ECatalogue", culture = "tr" });

app.MapControllerRoute(
    name: "news",
    pattern: "haberler",
    defaults: new { controller = "News", action = "Index", culture = "tr" });

app.MapControllerRoute(
    name: "news-detail",
    pattern: "haber/{slug}",
    defaults: new { controller = "News", action = "Detail", culture = "tr" });

app.MapControllerRoute(
    name: "about",
    pattern: "hakkimizda",
    defaults: new { controller = "Home", action = "About", culture = "tr" });

app.MapControllerRoute(
    name: "contact",
    pattern: "iletisim",
    defaults: new { controller = "Contact", action = "Index", culture = "tr" });



// ========================================
// ADMIN AREA ROUTES
// ========================================
app.MapControllerRoute(
    name: "admin-login",
    pattern: "admin/giris",
    defaults: new { area = "Admin", controller = "Auth", action = "Login" });

app.MapControllerRoute(
    name: "admin-logout",
    pattern: "admin/cikis",
    defaults: new { area = "Admin", controller = "Auth", action = "Logout" });

app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Health check endpoint for Docker/Coolify
app.MapHealthChecks("/health");

app.Run();
