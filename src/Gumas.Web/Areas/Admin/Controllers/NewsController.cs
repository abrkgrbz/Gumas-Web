using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gumas.Domain.Entities;
using Gumas.Domain.Interfaces;
using Gumas.Web.Extensions;

namespace Gumas.Web.Areas.Admin.Controllers;

public class NewsController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public NewsController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        _unitOfWork = unitOfWork;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var news = await _unitOfWork.News.GetAllAsync(
            orderBy: q => q.OrderByDescending(n => n.PublishDate));
        return View(news);
    }

    public IActionResult Create()
    {
        return View(new News { PublishDate = DateTime.Now, IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(News news, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            news.Slug = news.NameTr.ToSlug();

            if (imageFile != null)
            {
                news.ImageUrl = await SaveImageAsync(imageFile);
            }

            await _unitOfWork.News.AddAsync(news);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Haber başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }
        return View(news);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var news = await _unitOfWork.News.GetByIdAsync(id);
        if (news == null)
            return NotFound();

        return View(news);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, News news, IFormFile? imageFile)
    {
        if (id != news.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var existingNews = await _unitOfWork.News.GetByIdAsync(id);
            if (existingNews == null)
                return NotFound();

            existingNews.NameTr = news.NameTr;
            existingNews.NameEn = news.NameEn;
            existingNews.ContentTr = news.ContentTr;
            existingNews.ContentEn = news.ContentEn;
            existingNews.SummaryTr = news.SummaryTr;
            existingNews.SummaryEn = news.SummaryEn;
            existingNews.PublishDate = news.PublishDate;
            existingNews.IsActive = news.IsActive;
            existingNews.Slug = news.NameTr.ToSlug();
            existingNews.UpdatedAt = DateTime.UtcNow;

            if (imageFile != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(existingNews.ImageUrl))
                {
                    DeleteImage(existingNews.ImageUrl);
                }
                existingNews.ImageUrl = await SaveImageAsync(imageFile);
            }

            _unitOfWork.News.Update(existingNews);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Haber başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        return View(news);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var news = await _unitOfWork.News.GetByIdAsync(id);
        if (news == null)
            return NotFound();

        if (!string.IsNullOrEmpty(news.ImageUrl))
        {
            DeleteImage(news.ImageUrl);
        }

        _unitOfWork.News.Remove(news);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Haber başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var news = await _unitOfWork.News.GetByIdAsync(id);
        if (news == null)
            return NotFound();

        news.IsActive = !news.IsActive;
        news.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.News.Update(news);
        await _unitOfWork.SaveChangesAsync();

        return Json(new { success = true, isActive = news.IsActive });
    }

    private async Task<string> SaveImageAsync(IFormFile file)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "news");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/news/{uniqueFileName}";
    }

    private void DeleteImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        var filePath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
}
