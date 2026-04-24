using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gumas.Infrastructure.Data;
using Gumas.Web.ViewModels;

namespace Gumas.Web.Controllers;

public class NewsController : Controller
{
    private readonly GumasDbContext _context;

    public NewsController(GumasDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 9;

        var query = _context.News
            .Where(n => n.IsActive)
            .OrderByDescending(n => n.PublishDate);

        var totalCount = await query.CountAsync();

        var news = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new NewsListViewModel
        {
            News = news,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return View(viewModel);
    }

    [Route("haber/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var news = await _context.News
            .FirstOrDefaultAsync(n => n.Slug == slug && n.IsActive);

        if (news == null)
        {
            return NotFound();
        }

        var relatedNews = await _context.News
            .Where(n => n.IsActive && n.Id != news.Id)
            .OrderByDescending(n => n.PublishDate)
            .Take(3)
            .ToListAsync();

        var viewModel = new NewsDetailViewModel
        {
            News = news,
            RelatedNews = relatedNews
        };

        return View(viewModel);
    }
}
