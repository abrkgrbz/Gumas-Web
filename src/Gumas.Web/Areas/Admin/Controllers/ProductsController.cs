using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Domain.Interfaces;
using Gumas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gumas.Web.Areas.Admin.Controllers;

public class ProductsController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string? search, int? brandId, int? categoryId, int page = 1)
    {
        const int pageSize = 20;

        var products = await _unitOfWork.Products.GetAllAsync(
            filter: p => (string.IsNullOrEmpty(search) || p.NameTr.Contains(search) || p.NameEn.Contains(search) || (p.ProductCode != null && p.ProductCode.Contains(search)))
                      && (!brandId.HasValue || p.BrandId == brandId)
                      && (!categoryId.HasValue || p.CategoryId == categoryId),
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            includeProperties: "Brand,Category,Images",
            skip: (page - 1) * pageSize,
            take: pageSize);

        var totalCount = await _unitOfWork.Products.CountAsync(
            p => (string.IsNullOrEmpty(search) || p.NameTr.Contains(search) || p.NameEn.Contains(search))
              && (!brandId.HasValue || p.BrandId == brandId)
              && (!categoryId.HasValue || p.CategoryId == categoryId));

        ViewBag.Search = search;
        ViewBag.BrandId = brandId;
        ViewBag.CategoryId = categoryId;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        await LoadSelectLists();

        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        await LoadSelectLists();
        return View(new ProductCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return View(dto);
        }

        var existingProduct = await _unitOfWork.Products.GetAsync(p => p.Slug == dto.Slug);
        if (existingProduct != null)
        {
            ModelState.AddModelError("Slug", "Bu slug zaten kullanılıyor.");
            await LoadSelectLists();
            return View(dto);
        }

        var product = _mapper.Map<Product>(dto);
        product.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Ürün başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var dto = _mapper.Map<ProductCreateDto>(product);
        await LoadSelectLists();
        ViewBag.ProductId = id;

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            ViewBag.ProductId = id;
            return View(dto);
        }

        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var existingProduct = await _unitOfWork.Products.GetAsync(p => p.Slug == dto.Slug && p.Id != id);
        if (existingProduct != null)
        {
            ModelState.AddModelError("Slug", "Bu slug zaten kullanılıyor.");
            await LoadSelectLists();
            ViewBag.ProductId = id;
            return View(dto);
        }

        _mapper.Map(dto, product);
        product.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Ürün başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _unitOfWork.Products.Remove(product);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Ürün başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.IsActive = !product.IsActive;
        product.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFeatured(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.IsFeatured = !product.IsFeatured;
        product.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadSelectLists()
    {
        var brands = await _unitOfWork.Brands.GetAllAsync(filter: b => b.IsActive, orderBy: q => q.OrderBy(b => b.NameTr));
        var categories = await _unitOfWork.Categories.GetAllAsync(filter: c => c.IsActive, orderBy: q => q.OrderBy(c => c.NameTr));

        ViewBag.Brands = new SelectList(brands, "Id", "NameTr");
        ViewBag.Categories = new SelectList(categories, "Id", "NameTr");
    }
}
