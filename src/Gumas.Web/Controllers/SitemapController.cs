using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gumas.Infrastructure.Data;
using System.Text;
using System.Xml.Linq;

namespace Gumas.Web.Controllers;

public class SitemapController : Controller
{
    private readonly GumasDbContext _context;
    private readonly IConfiguration _configuration;

    public SitemapController(GumasDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [Route("sitemap.xml")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> Index()
    {
        var baseUrl = _configuration["AppSettings:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        XNamespace xhtml = "http://www.w3.org/1999/xhtml";

        var urlElements = new List<XElement>();

        // Static pages - Turkish
        var staticPagesTr = new[]
        {
            ("/", "1.0", "daily"),
            ("/urunler", "0.9", "daily"),
            ("/markalar", "0.8", "weekly"),
            ("/haberler", "0.8", "daily"),
            ("/hakkimizda", "0.6", "monthly"),
            ("/iletisim", "0.6", "monthly")
        };

        // Static pages - English
        var staticPagesEn = new[]
        {
            ("/en", "1.0", "daily"),
            ("/en/products", "0.9", "daily"),
            ("/en/brands", "0.8", "weekly"),
            ("/en/news", "0.8", "daily"),
            ("/en/about", "0.6", "monthly"),
            ("/en/contact", "0.6", "monthly")
        };

        // Add static pages with hreflang alternates
        for (int i = 0; i < staticPagesTr.Length; i++)
        {
            var trUrl = staticPagesTr[i];
            var enUrl = staticPagesEn[i];

            // Turkish version
            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + trUrl.Item1),
                new XElement(ns + "lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd")),
                new XElement(ns + "changefreq", trUrl.Item3),
                new XElement(ns + "priority", trUrl.Item2),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl.Item1)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl.Item1))
            ));

            // English version
            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + enUrl.Item1),
                new XElement(ns + "lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd")),
                new XElement(ns + "changefreq", enUrl.Item3),
                new XElement(ns + "priority", enUrl.Item2),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl.Item1)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl.Item1))
            ));
        }

        // Dynamic: Products
        var products = await _context.Products
            .Where(p => p.IsActive)
            .Select(p => new { p.Slug, p.UpdatedAt })
            .ToListAsync();

        foreach (var product in products)
        {
            var trUrl = $"/urun/{product.Slug}";
            var enUrl = $"/en/product/{product.Slug}";
            var lastMod = (product.UpdatedAt ?? DateTime.UtcNow).ToString("yyyy-MM-dd");

            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + trUrl),
                new XElement(ns + "lastmod", lastMod),
                new XElement(ns + "changefreq", "weekly"),
                new XElement(ns + "priority", "0.7"),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl))
            ));

            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + enUrl),
                new XElement(ns + "lastmod", lastMod),
                new XElement(ns + "changefreq", "weekly"),
                new XElement(ns + "priority", "0.7"),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl))
            ));
        }

        // Dynamic: Brands
        var brands = await _context.Brands
            .Where(b => b.IsActive)
            .Select(b => new { b.Slug, b.UpdatedAt })
            .ToListAsync();

        foreach (var brand in brands)
        {
            var trUrl = $"/marka/{brand.Slug}";
            var enUrl = $"/en/brand/{brand.Slug}";
            var lastMod = (brand.UpdatedAt ?? DateTime.UtcNow).ToString("yyyy-MM-dd");

            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + trUrl),
                new XElement(ns + "lastmod", lastMod),
                new XElement(ns + "changefreq", "weekly"),
                new XElement(ns + "priority", "0.6"),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl))
            ));

            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + enUrl),
                new XElement(ns + "lastmod", lastMod),
                new XElement(ns + "changefreq", "weekly"),
                new XElement(ns + "priority", "0.6"),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl))
            ));
        }

        // Dynamic: News
        var news = await _context.News
            .Where(n => n.IsActive)
            .Select(n => new { n.Slug, n.UpdatedAt })
            .ToListAsync();

        foreach (var item in news)
        {
            var trUrl = $"/haber/{item.Slug}";
            var enUrl = $"/en/news/{item.Slug}";
            var lastMod = (item.UpdatedAt ?? DateTime.UtcNow).ToString("yyyy-MM-dd");

            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + trUrl),
                new XElement(ns + "lastmod", lastMod),
                new XElement(ns + "changefreq", "monthly"),
                new XElement(ns + "priority", "0.5"),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl))
            ));

            urlElements.Add(new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + enUrl),
                new XElement(ns + "lastmod", lastMod),
                new XElement(ns + "changefreq", "monthly"),
                new XElement(ns + "priority", "0.5"),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "tr"),
                    new XAttribute("href", baseUrl + trUrl)),
                new XElement(xhtml + "link",
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", "en"),
                    new XAttribute("href", baseUrl + enUrl))
            ));
        }

        var sitemap = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement(ns + "urlset",
                new XAttribute(XNamespace.Xmlns + "xhtml", xhtml),
                urlElements
            )
        );

        return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
    }
}
