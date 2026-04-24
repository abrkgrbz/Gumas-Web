using Gumas.Domain.Interfaces;
using Gumas.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gumas.Web.Areas.Admin.Controllers;

public class DashboardController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var productCount = await _unitOfWork.Products.CountAsync();
        var brandCount = await _unitOfWork.Brands.CountAsync();
        var categoryCount = await _unitOfWork.Categories.CountAsync();
        var newsCount = await _unitOfWork.News.CountAsync();
        var sliderCount = await _unitOfWork.Sliders.CountAsync();
        var messageCount = await _unitOfWork.ContactMessages.CountAsync();
        var unreadMessageCount = await _unitOfWork.ContactMessages.CountAsync(m => m.Status == MessageStatus.New);

        ViewBag.ProductCount = productCount;
        ViewBag.BrandCount = brandCount;
        ViewBag.CategoryCount = categoryCount;
        ViewBag.NewsCount = newsCount;
        ViewBag.SliderCount = sliderCount;
        ViewBag.MessageCount = messageCount;
        ViewBag.UnreadMessageCount = unreadMessageCount;

        // Son eklenen ürünler
        var recentProducts = await _unitOfWork.Products.GetAllAsync(
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            take: 5,
            includeProperties: "Brand,Category");

        // Son gelen mesajlar
        var recentMessages = await _unitOfWork.ContactMessages.GetAllAsync(
            orderBy: q => q.OrderByDescending(m => m.CreatedAt),
            take: 5);

        ViewBag.RecentProducts = recentProducts;
        ViewBag.RecentMessages = recentMessages;

        return View();
    }
}
