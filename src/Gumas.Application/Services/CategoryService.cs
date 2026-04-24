using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Domain.Entities;
using Gumas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gumas.Application.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync(string culture);
    Task<List<CategoryListDto>> GetCategoryListAsync(string culture);
    Task<List<CategoryDto>> GetCategoriesWithChildrenAsync(string culture);
    Task<CategoryDto?> GetCategoryBySlugAsync(string slug, string culture);
    Task<CategoryDto?> GetCategoryByIdAsync(int id, string culture);
    Task<Category> CreateAsync(CategoryCreateDto dto);
    Task UpdateAsync(int id, CategoryCreateDto dto);
    Task DeleteAsync(int id);
}

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync(string culture)
    {
        var categories = await _unitOfWork.Categories.Query()
            .Include(c => c.Products)
            .Include(c => c.Parent)
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return categories.Select(c => MapCategoryDto(c, culture)).ToList();
    }

    public async Task<List<CategoryListDto>> GetCategoryListAsync(string culture)
    {
        var categories = await _unitOfWork.Categories.Query()
            .Include(c => c.Products)
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return categories.Select(c => MapCategoryListDto(c, culture)).ToList();
    }

    public async Task<List<CategoryDto>> GetCategoriesWithChildrenAsync(string culture)
    {
        var categories = await _unitOfWork.Categories.Query()
            .Include(c => c.Products)
            .Include(c => c.Children).ThenInclude(c => c.Products)
            .Where(c => c.IsActive && c.ParentId == null)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return categories.Select(c => MapCategoryDtoWithChildren(c, culture)).ToList();
    }

    public async Task<CategoryDto?> GetCategoryBySlugAsync(string slug, string culture)
    {
        var category = await _unitOfWork.Categories.Query()
            .Include(c => c.Products)
            .Include(c => c.Parent)
            .Include(c => c.Children).ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);

        return category == null ? null : MapCategoryDtoWithChildren(category, culture);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id, string culture)
    {
        var category = await _unitOfWork.Categories.Query()
            .Include(c => c.Products)
            .Include(c => c.Parent)
            .Include(c => c.Children).ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        return category == null ? null : MapCategoryDtoWithChildren(category, culture);
    }

    public async Task<Category> CreateAsync(CategoryCreateDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(int id, CategoryCreateDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id)
            ?? throw new Exception("Category not found");

        _mapper.Map(dto, category);
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id)
            ?? throw new Exception("Category not found");

        _unitOfWork.Categories.Remove(category);
        await _unitOfWork.SaveChangesAsync();
    }

    private CategoryListDto MapCategoryListDto(Category category, string culture)
    {
        var dto = _mapper.Map<CategoryListDto>(category);
        dto.Name = category.GetName(culture);
        return dto;
    }

    private CategoryDto MapCategoryDto(Category category, string culture)
    {
        var dto = _mapper.Map<CategoryDto>(category);
        dto.Name = category.GetName(culture);
        dto.Description = category.GetDescription(culture);
        dto.ParentName = category.Parent?.GetName(culture);
        dto.MetaTitle = culture == "en" ? category.MetaTitleEn : category.MetaTitleTr;
        dto.MetaDescription = culture == "en" ? category.MetaDescriptionEn : category.MetaDescriptionTr;
        return dto;
    }

    private CategoryDto MapCategoryDtoWithChildren(Category category, string culture)
    {
        var dto = MapCategoryDto(category, culture);
        dto.Children = category.Children
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => MapCategoryDto(c, culture))
            .ToList();
        return dto;
    }
}
