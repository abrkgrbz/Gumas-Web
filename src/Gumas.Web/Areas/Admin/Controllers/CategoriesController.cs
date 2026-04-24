using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Gumas.Domain.Entities;
using Gumas.Domain.Interfaces;
using Gumas.Web.Extensions;

namespace Gumas.Web.Areas.Admin.Controllers;

public class CategoriesController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public CategoriesController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        _unitOfWork = unitOfWork;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(
            orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.NameTr),
            includeProperties: "Parent,Children");
        return View(categories);
    }

    public async Task<IActionResult> Create()
    {
        await LoadParentCategoriesAsync();
        return View(new Category { IsActive = true, DisplayOrder = 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            category.Slug = category.NameTr.ToSlug();

            if (imageFile != null)
            {
                category.ImageUrl = await SaveImageAsync(imageFile);
            }

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        await LoadParentCategoriesAsync(category.ParentId);
        return View(category);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        await LoadParentCategoriesAsync(category.ParentId, id);
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category, IFormFile? imageFile)
    {
        if (id != category.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            existingCategory.NameTr = category.NameTr;
            existingCategory.NameEn = category.NameEn;
            existingCategory.DescriptionTr = category.DescriptionTr;
            existingCategory.DescriptionEn = category.DescriptionEn;
            existingCategory.ParentId = category.ParentId;
            existingCategory.DisplayOrder = category.DisplayOrder;
            existingCategory.IsActive = category.IsActive;
            existingCategory.Slug = category.NameTr.ToSlug();
            existingCategory.UpdatedAt = DateTime.UtcNow;

            if (imageFile != null)
            {
                if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                {
                    DeleteImage(existingCategory.ImageUrl);
                }
                existingCategory.ImageUrl = await SaveImageAsync(imageFile);
            }

            _unitOfWork.Categories.Update(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        await LoadParentCategoriesAsync(category.ParentId, id);
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        // Check for children
        var hasChildren = await _unitOfWork.Categories.AnyAsync(c => c.ParentId == id);
        if (hasChildren)
        {
            TempData["Error"] = "Alt kategorileri olan bir kategori silinemez.";
            return RedirectToAction(nameof(Index));
        }

        // Check for products
        var hasProducts = await _unitOfWork.Products.AnyAsync(p => p.CategoryId == id);
        if (hasProducts)
        {
            TempData["Error"] = "Ürün içeren bir kategori silinemez.";
            return RedirectToAction(nameof(Index));
        }

        if (!string.IsNullOrEmpty(category.ImageUrl))
        {
            DeleteImage(category.ImageUrl);
        }

        _unitOfWork.Categories.Remove(category);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Kategori başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        category.IsActive = !category.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return Json(new { success = true, isActive = category.IsActive });
    }

    private async Task LoadParentCategoriesAsync(int? selectedId = null, int? excludeId = null)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(
            filter: c => c.ParentId == null && (excludeId == null || c.Id != excludeId),
            orderBy: q => q.OrderBy(c => c.DisplayOrder));

        ViewBag.ParentCategories = new SelectList(categories, "Id", "NameTr", selectedId);
    }

    private async Task<string> SaveImageAsync(IFormFile file)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "categories");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/categories/{uniqueFileName}";
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
