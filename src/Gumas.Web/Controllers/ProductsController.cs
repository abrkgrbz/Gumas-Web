using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gumas.Infrastructure.Data;
using Gumas.Web.ViewModels;

namespace Gumas.Web.Controllers;

public class ProductsController : Controller
{
    private readonly GumasDbContext _context;

    public ProductsController(GumasDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId, int? brandId, int page = 1)
    {
        const int pageSize = 24;

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        var brands = await _context.Brands
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();

        var categoryProductCounts = await _context.Products
            .Where(p => p.IsActive)
            .GroupBy(p => p.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

        var query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value);

        var totalCount = await query.CountAsync();

        var products = await query
            .OrderBy(p => p.DisplayOrder)
            .ThenByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new ProductsViewModel
        {
            Products = products,
            Categories = categories,
            Brands = brands,
            CategoryProductCounts = categoryProductCounts,
            SelectedCategoryId = categoryId,
            SelectedBrandId = brandId,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return View(viewModel);
    }

    [Route("urun/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

        if (product == null)
        {
            return NotFound();
        }

        var relatedProducts = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.Id != product.Id &&
                       (p.CategoryId == product.CategoryId || p.BrandId == product.BrandId))
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
            .ToListAsync();

        var viewModel = new ProductDetailViewModel
        {
            Product = product,
            RelatedProducts = relatedProducts
        };

        return View(viewModel);
    }
}
