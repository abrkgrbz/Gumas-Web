using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gumas.Infrastructure.Data;
using Gumas.Web.ViewModels;

namespace Gumas.Web.Controllers;

public class BrandsController : Controller
{
    private readonly GumasDbContext _context;

    public BrandsController(GumasDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var brands = await _context.Brands
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ThenBy(b => b.NameTr)
            .ToListAsync();

        var viewModel = new BrandsViewModel
        {
            Brands = brands,
            TotalCount = brands.Count()
        };

        return View(viewModel);
    }

    // SIRIT: 3.revize.pdf Madde 7 sonrası PDF kataloguna yönlendirme.
    private static readonly Dictionary<string, string> ExternalBrandRedirects = new(StringComparer.OrdinalIgnoreCase)
    {
        { "gerflor", "https://www.gerflortransport.com/en" },
        { "sirit", "https://www.sirit.it/eng/catalogo_generale_sirit.pdf" }
    };

    public IActionResult ECatalogue()
    {
        // 3.revize.pdf Madde 5: EN'de E-Catalogue doğrudan JOST-world'e yönlendirilir,
        // yerel Türkçe e-katalog sayfası İngilizce kültürde gösterilmez.
        var culture = RouteData.Values["culture"]?.ToString();
        if (string.Equals(culture, "en", StringComparison.OrdinalIgnoreCase))
        {
            return Redirect("https://www.jost-world.com/en/products/e-catalogue.html");
        }

        return View();
    }

    public async Task<IActionResult> Detail(string slug, int page = 1)
    {
        const int pageSize = 12;

        if (!string.IsNullOrEmpty(slug) && ExternalBrandRedirects.TryGetValue(slug, out var externalUrl))
        {
            return Redirect(externalUrl);
        }

        var brand = await _context.Brands
            .FirstOrDefaultAsync(b => b.Slug == slug && b.IsActive);

        if (brand == null)
        {
            return NotFound();
        }

        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.BrandId == brand.Id);

        var totalCount = await query.CountAsync();

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new BrandDetailViewModel
        {
            Brand = brand,
            Products = products,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return View(viewModel);
    }
}
