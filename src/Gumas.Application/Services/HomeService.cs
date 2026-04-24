using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Application.ViewModels;
using Gumas.Domain.Enums;
using Gumas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gumas.Application.Services;

public interface IHomeService
{
    Task<HomeViewModel> GetHomeDataAsync(string culture);
    Task<List<SliderDto>> GetSlidersAsync(SliderType type, string culture);
    Task<Dictionary<string, string>> GetSettingsAsync(string? groupName, string culture);
}

public class HomeService : IHomeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;

    public HomeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IProductService productService,
        IBrandService brandService,
        ICategoryService categoryService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _productService = productService;
        _brandService = brandService;
        _categoryService = categoryService;
    }

    public async Task<HomeViewModel> GetHomeDataAsync(string culture)
    {
        var viewModel = new HomeViewModel
        {
            Sliders = await GetSlidersAsync(SliderType.Hero, culture),
            FeaturedBrands = await _brandService.GetFeaturedBrandsAsync(culture),
            FeaturedProducts = await _productService.GetFeaturedProductsAsync(8, culture),
            Categories = await _categoryService.GetCategoryListAsync(culture),
            LatestNews = await GetLatestNewsAsync(3, culture),
            Statistics = await GetSettingsAsync("Statistics", culture)
        };

        return viewModel;
    }

    public async Task<List<SliderDto>> GetSlidersAsync(SliderType type, string culture)
    {
        var now = DateTime.UtcNow;
        var sliders = await _unitOfWork.Sliders.Query()
            .Where(s => s.IsActive && s.Type == type &&
                       (s.StartDate == null || s.StartDate <= now) &&
                       (s.EndDate == null || s.EndDate >= now))
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();

        return sliders.Select(s => MapSliderDto(s, culture)).ToList();
    }

    public async Task<Dictionary<string, string>> GetSettingsAsync(string? groupName, string culture)
    {
        var query = _unitOfWork.Settings.Query().Where(s => s.IsActive);

        if (!string.IsNullOrEmpty(groupName))
            query = query.Where(s => s.GroupName == groupName);

        var settings = await query.ToListAsync();

        return settings.ToDictionary(
            s => s.Key,
            s => s.GetValue(culture) ?? string.Empty);
    }

    private async Task<List<NewsListDto>> GetLatestNewsAsync(int count, string culture)
    {
        var news = await _unitOfWork.News.Query()
            .Where(n => n.IsActive && n.PublishDate <= DateTime.UtcNow)
            .OrderByDescending(n => n.PublishDate)
            .Take(count)
            .ToListAsync();

        return news.Select(n => new NewsListDto
        {
            Id = n.Id,
            Name = n.GetName(culture),
            Slug = n.Slug,
            Summary = n.GetSummary(culture),
            ThumbnailUrl = n.ThumbnailUrl,
            PublishDate = n.PublishDate
        }).ToList();
    }

    private SliderDto MapSliderDto(Domain.Entities.Slider slider, string culture)
    {
        return new SliderDto
        {
            Id = slider.Id,
            Name = slider.GetName(culture),
            Subtitle = slider.GetSubtitle(culture),
            Description = slider.GetDescription(culture),
            ImageUrl = slider.ImageUrl,
            MobileImageUrl = slider.MobileImageUrl,
            VideoUrl = slider.VideoUrl,
            ButtonText = slider.GetButtonText(culture),
            ButtonUrl = slider.ButtonUrl,
            Type = slider.Type,
            DisplayOrder = slider.DisplayOrder
        };
    }
}
