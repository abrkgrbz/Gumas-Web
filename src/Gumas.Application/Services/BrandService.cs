using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Domain.Entities;
using Gumas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gumas.Application.Services;

public interface IBrandService
{
    Task<List<BrandListDto>> GetAllBrandsAsync(string culture);
    Task<List<BrandListDto>> GetFeaturedBrandsAsync(string culture);
    Task<BrandDto?> GetBrandBySlugAsync(string slug, string culture);
    Task<BrandDto?> GetBrandByIdAsync(int id, string culture);
    Task<Brand> CreateAsync(BrandCreateDto dto);
    Task UpdateAsync(int id, BrandCreateDto dto);
    Task DeleteAsync(int id);

    // Admin methods
    Task CreateBrandAsync(BrandDto dto);
    Task UpdateBrandAsync(BrandDto dto);
    Task DeleteBrandAsync(int id);
}

public class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<BrandListDto>> GetAllBrandsAsync(string culture)
    {
        var brands = await _unitOfWork.Brands.Query()
            .Include(b => b.Products)
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();

        return brands.Select(b => MapBrandListDto(b, culture)).ToList();
    }

    public async Task<List<BrandListDto>> GetFeaturedBrandsAsync(string culture)
    {
        var brands = await _unitOfWork.Brands.Query()
            .Include(b => b.Products)
            .Where(b => b.IsActive && b.IsFeatured)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();

        return brands.Select(b => MapBrandListDto(b, culture)).ToList();
    }

    public async Task<BrandDto?> GetBrandBySlugAsync(string slug, string culture)
    {
        var brand = await _unitOfWork.Brands.Query()
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Slug == slug && b.IsActive);

        return brand == null ? null : MapBrandDto(brand, culture);
    }

    public async Task<BrandDto?> GetBrandByIdAsync(int id, string culture)
    {
        var brand = await _unitOfWork.Brands.Query()
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Id == id);

        return brand == null ? null : MapBrandDto(brand, culture);
    }

    public async Task<Brand> CreateAsync(BrandCreateDto dto)
    {
        var brand = _mapper.Map<Brand>(dto);
        await _unitOfWork.Brands.AddAsync(brand);
        await _unitOfWork.SaveChangesAsync();
        return brand;
    }

    public async Task UpdateAsync(int id, BrandCreateDto dto)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id)
            ?? throw new Exception("Brand not found");

        _mapper.Map(dto, brand);
        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id)
            ?? throw new Exception("Brand not found");

        _unitOfWork.Brands.Remove(brand);
        await _unitOfWork.SaveChangesAsync();
    }

    // Admin methods
    public async Task CreateBrandAsync(BrandDto dto)
    {
        var brand = new Brand
        {
            NameTr = dto.NameTr,
            NameEn = dto.NameEn,
            Slug = dto.Slug,
            DescriptionTr = dto.DescriptionTr,
            DescriptionEn = dto.DescriptionEn,
            LongDescriptionTr = dto.LongDescriptionTr,
            LongDescriptionEn = dto.LongDescriptionEn,
            LogoUrl = dto.LogoUrl,
            BannerUrl = dto.BannerUrl,
            Website = dto.Website ?? dto.WebsiteUrl,
            Country = dto.Country,
            FoundedYear = dto.FoundedYear,
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive,
            IsFeatured = dto.IsFeatured,
            MetaTitleTr = dto.MetaTitleTr,
            MetaTitleEn = dto.MetaTitleEn,
            MetaDescriptionTr = dto.MetaDescriptionTr,
            MetaDescriptionEn = dto.MetaDescriptionEn
        };

        await _unitOfWork.Brands.AddAsync(brand);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateBrandAsync(BrandDto dto)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(dto.Id)
            ?? throw new Exception("Brand not found");

        brand.NameTr = dto.NameTr;
        brand.NameEn = dto.NameEn;
        brand.Slug = dto.Slug;
        brand.DescriptionTr = dto.DescriptionTr;
        brand.DescriptionEn = dto.DescriptionEn;
        brand.LongDescriptionTr = dto.LongDescriptionTr;
        brand.LongDescriptionEn = dto.LongDescriptionEn;
        brand.LogoUrl = dto.LogoUrl;
        brand.BannerUrl = dto.BannerUrl;
        brand.Website = dto.Website ?? dto.WebsiteUrl;
        brand.Country = dto.Country;
        brand.FoundedYear = dto.FoundedYear;
        brand.DisplayOrder = dto.DisplayOrder;
        brand.IsActive = dto.IsActive;
        brand.IsFeatured = dto.IsFeatured;
        brand.MetaTitleTr = dto.MetaTitleTr;
        brand.MetaTitleEn = dto.MetaTitleEn;
        brand.MetaDescriptionTr = dto.MetaDescriptionTr;
        brand.MetaDescriptionEn = dto.MetaDescriptionEn;

        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBrandAsync(int id)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id)
            ?? throw new Exception("Brand not found");

        // Soft delete
        brand.IsActive = false;
        brand.IsDeleted = true;
        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync();
    }

    private BrandListDto MapBrandListDto(Brand brand, string culture)
    {
        return new BrandListDto
        {
            Id = brand.Id,
            Name = brand.GetName(culture),
            NameTr = brand.NameTr,
            NameEn = brand.NameEn,
            Slug = brand.Slug,
            LogoUrl = brand.LogoUrl,
            ProductCount = brand.Products?.Count(p => p.IsActive) ?? 0
        };
    }

    private BrandDto MapBrandDto(Brand brand, string culture)
    {
        return new BrandDto
        {
            Id = brand.Id,
            Name = brand.GetName(culture),
            NameTr = brand.NameTr,
            NameEn = brand.NameEn,
            Slug = brand.Slug,
            Description = brand.GetDescription(culture),
            DescriptionTr = brand.DescriptionTr,
            DescriptionEn = brand.DescriptionEn,
            LongDescription = brand.GetLongDescription(culture),
            LongDescriptionTr = brand.LongDescriptionTr,
            LongDescriptionEn = brand.LongDescriptionEn,
            LogoUrl = brand.LogoUrl,
            BannerUrl = brand.BannerUrl,
            Website = brand.Website,
            WebsiteUrl = brand.Website,
            Country = brand.Country,
            FoundedYear = brand.FoundedYear,
            DisplayOrder = brand.DisplayOrder,
            IsActive = brand.IsActive,
            IsFeatured = brand.IsFeatured,
            ProductCount = brand.Products?.Count(p => p.IsActive) ?? 0,
            MetaTitle = culture == "en" ? brand.MetaTitleEn : brand.MetaTitleTr,
            MetaTitleTr = brand.MetaTitleTr,
            MetaTitleEn = brand.MetaTitleEn,
            MetaDescription = culture == "en" ? brand.MetaDescriptionEn : brand.MetaDescriptionTr,
            MetaDescriptionTr = brand.MetaDescriptionTr,
            MetaDescriptionEn = brand.MetaDescriptionEn
        };
    }
}
