using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Gumas.Web.Controllers;

public class LanguageController : Controller
{
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        // Validate culture
        if (culture != "tr" && culture != "en")
        {
            culture = "tr";
        }

        // Set cookie
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            }
        );

        // Convert URL based on language
        var newUrl = ConvertUrl(returnUrl ?? "/", culture);
        return LocalRedirect(newUrl);
    }

    private string ConvertUrl(string url, string targetCulture)
    {
        // Handle null or empty
        if (string.IsNullOrEmpty(url) || url == "/")
        {
            return targetCulture == "en" ? "/en" : "/";
        }

        // URL mapping dictionary: key -> (Turkish, English)
        var urlMappings = new Dictionary<string, (string Turkish, string English)>
        {
            { "products", ("/urunler", "/en/products") },
            { "urunler", ("/urunler", "/en/products") },
            { "product/", ("/urun/", "/en/product/") },
            { "urun/", ("/urun/", "/en/product/") },
            { "brands", ("/markalar", "/en/brands") },
            { "markalar", ("/markalar", "/en/brands") },
            { "brand/", ("/marka/", "/en/brand/") },
            { "marka/", ("/marka/", "/en/brand/") },
            { "news", ("/haberler", "/en/news") },
            { "haberler", ("/haberler", "/en/news") },
            { "about", ("/hakkimizda", "/en/about") },
            { "hakkimizda", ("/hakkimizda", "/en/about") },
            { "contact", ("/iletisim", "/en/contact") },
            { "iletisim", ("/iletisim", "/en/contact") },
        };

        // Clean URL
        var cleanUrl = url.TrimStart('/');

        // Remove /en prefix if exists
        if (cleanUrl.StartsWith("en/"))
        {
            cleanUrl = cleanUrl.Substring(3);
        }
        else if (cleanUrl == "en")
        {
            return targetCulture == "en" ? "/en" : "/";
        }

        // Try to find a matching pattern
        foreach (var mapping in urlMappings)
        {
            if (cleanUrl.StartsWith(mapping.Key, StringComparison.OrdinalIgnoreCase))
            {
                var remainder = cleanUrl.Substring(mapping.Key.Length);
                var baseUrl = targetCulture == "en" ? mapping.Value.English : mapping.Value.Turkish;
                return baseUrl + remainder;
            }
        }

        // If no mapping found, just add/remove /en prefix
        return targetCulture == "en" ? $"/en/{cleanUrl}" : $"/{cleanUrl}";
    }
}
