using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Domain.Interfaces;
using Gumas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gumas.Web.Areas.Admin.Controllers;

public class BrandsController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BrandsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 20;

        var brands = await _unitOfWork.Brands.GetAllAsync(
            filter: b => string.IsNullOrEmpty(search) || b.NameTr.Contains(search) || b.NameEn.Contains(search),
            orderBy: q => q.OrderBy(b => b.DisplayOrder).ThenBy(b => b.NameTr),
            skip: (page - 1) * pageSize,
            take: pageSize);

        var totalCount = await _unitOfWork.Brands.CountAsync(
            b => string.IsNullOrEmpty(search) || b.NameTr.Contains(search) || b.NameEn.Contains(search));

        ViewBag.Search = search;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return View(brands);
    }

    public IActionResult Create()
    {
        return View(new BrandCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var existingBrand = await _unitOfWork.Brands.GetAsync(b => b.Slug == dto.Slug);
        if (existingBrand != null)
        {
            ModelState.AddModelError("Slug", "Bu slug zaten kullanılıyor.");
            return View(dto);
        }

        var brand = _mapper.Map<Brand>(dto);
        brand.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Brands.AddAsync(brand);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Marka başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound();
        }

        var dto = _mapper.Map<BrandCreateDto>(brand);
        ViewBag.BrandId = id;

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BrandCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BrandId = id;
            return View(dto);
        }

        var brand = await _unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound();
        }

        var existingBrand = await _unitOfWork.Brands.GetAsync(b => b.Slug == dto.Slug && b.Id != id);
        if (existingBrand != null)
        {
            ModelState.AddModelError("Slug", "Bu slug zaten kullanılıyor.");
            ViewBag.BrandId = id;
            return View(dto);
        }

        _mapper.Map(dto, brand);
        brand.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Marka başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound();
        }

        var productCount = await _unitOfWork.Products.CountAsync(p => p.BrandId == id);
        if (productCount > 0)
        {
            TempData["Error"] = $"Bu markaya bağlı {productCount} ürün bulunmaktadır. Önce ürünleri silin veya başka bir markaya taşıyın.";
            return RedirectToAction(nameof(Index));
        }

        _unitOfWork.Brands.Remove(brand);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Marka başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound();
        }

        brand.IsActive = !brand.IsActive;
        brand.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFeatured(int id)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound();
        }

        brand.IsFeatured = !brand.IsFeatured;
        brand.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
