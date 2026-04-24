using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gumas.Infrastructure.Data;
using Gumas.Web.ViewModels;

namespace Gumas.Web.Controllers;

public class HomeController : Controller
{
    private readonly GumasDbContext _context;

    public HomeController(GumasDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            Sliders = await _context.Sliders
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync(),

            FeaturedBrands = await _context.Brands
                .Where(b => b.IsActive && b.IsFeatured)
                .OrderBy(b => b.DisplayOrder)
                .Take(8)
                .ToListAsync(),

            FeaturedProducts = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsActive && p.IsFeatured)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync(),

            LatestNews = await _context.News
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.PublishDate)
                .Take(3)
                .ToListAsync(),

            Categories = await _context.Categories
                .Where(c => c.IsActive && c.ParentId == null)
                .Include(c => c.Children.Where(sc => sc.IsActive))
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync()
        };

        return View(viewModel);
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
