using Gumas.Domain.Interfaces;
using Gumas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gumas.Web.Areas.Admin.Controllers;

public class SlidersController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public SlidersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _unitOfWork.Sliders.GetAllAsync(
            orderBy: q => q.OrderBy(s => s.DisplayOrder));

        return View(sliders);
    }

    public IActionResult Create()
    {
        return View(new Slider());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Slider slider)
    {
        if (!ModelState.IsValid)
        {
            return View(slider);
        }

        slider.CreatedAt = DateTime.UtcNow;
        await _unitOfWork.Sliders.AddAsync(slider);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Slider başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var slider = await _unitOfWork.Sliders.GetByIdAsync(id);
        if (slider == null)
        {
            return NotFound();
        }

        return View(slider);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Slider slider)
    {
        if (id != slider.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(slider);
        }

        var existingSlider = await _unitOfWork.Sliders.GetByIdAsync(id);
        if (existingSlider == null)
        {
            return NotFound();
        }

        existingSlider.TitleTr = slider.TitleTr;
        existingSlider.TitleEn = slider.TitleEn;
        existingSlider.SubtitleTr = slider.SubtitleTr;
        existingSlider.SubtitleEn = slider.SubtitleEn;
        existingSlider.ImageUrl = slider.ImageUrl;
        existingSlider.MobileImageUrl = slider.MobileImageUrl;
        existingSlider.ButtonUrl = slider.ButtonUrl;
        existingSlider.ButtonTextTr = slider.ButtonTextTr;
        existingSlider.ButtonTextEn = slider.ButtonTextEn;
        existingSlider.DisplayOrder = slider.DisplayOrder;
        existingSlider.IsActive = slider.IsActive;
        existingSlider.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Sliders.Update(existingSlider);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Slider başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var slider = await _unitOfWork.Sliders.GetByIdAsync(id);
        if (slider == null)
        {
            return NotFound();
        }

        _unitOfWork.Sliders.Remove(slider);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Slider başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var slider = await _unitOfWork.Sliders.GetByIdAsync(id);
        if (slider == null)
        {
            return NotFound();
        }

        slider.IsActive = !slider.IsActive;
        slider.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Sliders.Update(slider);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrder(int id, int newOrder)
    {
        var slider = await _unitOfWork.Sliders.GetByIdAsync(id);
        if (slider == null)
        {
            return NotFound();
        }

        slider.DisplayOrder = newOrder;
        slider.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Sliders.Update(slider);
        await _unitOfWork.SaveChangesAsync();

        return Ok();
    }
}
