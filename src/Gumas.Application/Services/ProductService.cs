using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Application.ViewModels;
using Gumas.Domain.Entities;
using Gumas.Domain.Enums;
using Gumas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gumas.Application.Services;

public interface IProductService
{
    Task<PagedResult<ProductListDto>> GetProductsAsync(ProductFilterDto filter, string culture);
    Task<ProductDto?> GetProductBySlugAsync(string slug, string culture);
    Task<ProductDto?> GetProductByIdAsync(int id, string culture);
    Task<List<ProductListDto>> GetFeaturedProductsAsync(int count, string culture);
    Task<List<ProductListDto>> GetRelatedProductsAsync(int productId, int count, string culture);
    Task<List<ProductListDto>> GetProductsByBrandAsync(int brandId, string culture, int? take = null);
    Task<List<ProductListDto>> GetProductsByCategoryAsync(int categoryId, string culture, int? take = null);
    Task IncrementViewCountAsync(int productId);
    Task<Product> CreateAsync(ProductCreateDto dto);
    Task UpdateAsync(int id, ProductCreateDto dto);
    Task DeleteAsync(int id);

    // Admin methods
    Task<PagedResult<ProductDto>> GetAllProductsAsync(string culture, int page = 1, int pageSize = 20);
    Task CreateProductAsync(ProductDto dto);
    Task UpdateProductAsync(ProductDto dto);
    Task DeleteProductAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductListDto>> GetProductsAsync(ProductFilterDto filter, string culture)
    {
        var query = _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.Status == ProductStatus.Active);

        // Apply filters
        if (filter.BrandId.HasValue)
            query = query.Where(p => p.BrandId == filter.BrandId.Value);

        if (filter.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

        if (filter.IsFeatured.HasValue)
            query = query.Where(p => p.IsFeatured == filter.IsFeatured.Value);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            query = query.Where(p =>
                p.NameTr.ToLower().Contains(term) ||
                p.NameEn.ToLower().Contains(term) ||
                (p.ProductCode != null && p.ProductCode.ToLower().Contains(term)) ||
                (p.OemNumber != null && p.OemNumber.ToLower().Contains(term)));
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "name" => filter.SortDescending ? query.OrderByDescending(p => p.NameTr) : query.OrderBy(p => p.NameTr),
            "name_desc" => query.OrderByDescending(p => p.NameTr),
            "date" => filter.SortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            "views" => filter.SortDescending ? query.OrderByDescending(p => p.ViewCount) : query.OrderBy(p => p.ViewCount),
            "brand" => query.OrderBy(p => p.Brand.NameTr),
            _ => query.OrderBy(p => p.DisplayOrder).ThenByDescending(p => p.CreatedAt)
        };

        // Apply pagination
        var products = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var dtos = products.Select(p => MapProductListDto(p, culture)).ToList();

        return new PagedResult<ProductListDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ProductDto?> GetProductBySlugAsync(string slug, string culture)
    {
        var product = await _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .Include(p => p.Documents.OrderBy(d => d.DisplayOrder))
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);

        return product == null ? null : MapProductDto(product, culture);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, string culture)
    {
        var product = await _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .Include(p => p.Documents.OrderBy(d => d.DisplayOrder))
            .FirstOrDefaultAsync(p => p.Id == id);

        return product == null ? null : MapProductDto(product, culture);
    }

    public async Task<List<ProductListDto>> GetFeaturedProductsAsync(int count, string culture)
    {
        var products = await _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.IsFeatured && p.Status == ProductStatus.Active)
            .OrderBy(p => p.DisplayOrder)
            .Take(count)
            .ToListAsync();

        return products.Select(p => MapProductListDto(p, culture)).ToList();
    }

    public async Task<List<ProductListDto>> GetRelatedProductsAsync(int productId, int count, string culture)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product == null) return new List<ProductListDto>();

        var products = await _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.Id != productId && p.IsActive && p.Status == ProductStatus.Active &&
                       (p.CategoryId == product.CategoryId || p.BrandId == product.BrandId))
            .OrderBy(p => p.DisplayOrder)
            .Take(count)
            .ToListAsync();

        return products.Select(p => MapProductListDto(p, culture)).ToList();
    }

    public async Task<List<ProductListDto>> GetProductsByBrandAsync(int brandId, string culture, int? take = null)
    {
        var query = _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.BrandId == brandId && p.IsActive && p.Status == ProductStatus.Active)
            .OrderBy(p => p.DisplayOrder);

        if (take.HasValue)
            query = (IOrderedQueryable<Product>)query.Take(take.Value);

        var products = await query.ToListAsync();
        return products.Select(p => MapProductListDto(p, culture)).ToList();
    }

    public async Task<List<ProductListDto>> GetProductsByCategoryAsync(int categoryId, string culture, int? take = null)
    {
        var query = _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.CategoryId == categoryId && p.IsActive && p.Status == ProductStatus.Active)
            .OrderBy(p => p.DisplayOrder);

        if (take.HasValue)
            query = (IOrderedQueryable<Product>)query.Take(take.Value);

        var products = await query.ToListAsync();
        return products.Select(p => MapProductListDto(p, culture)).ToList();
    }

    public async Task IncrementViewCountAsync(int productId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product != null)
        {
            product.ViewCount++;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<Product> CreateAsync(ProductCreateDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(int id, ProductCreateDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id)
            ?? throw new Exception("Product not found");

        _mapper.Map(dto, product);
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id)
            ?? throw new Exception("Product not found");

        _unitOfWork.Products.Remove(product);
        await _unitOfWork.SaveChangesAsync();
    }

    // Admin methods
    public async Task<PagedResult<ProductDto>> GetAllProductsAsync(string culture, int page = 1, int pageSize = 20)
    {
        var query = _unitOfWork.Products.Query()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = products.Select(p => MapProductDto(p, culture)).ToList();

        return new PagedResult<ProductDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task CreateProductAsync(ProductDto dto)
    {
        var product = new Product
        {
            NameTr = dto.NameTr,
            NameEn = dto.NameEn,
            Slug = dto.Slug,
            ProductCode = dto.ProductCode,
            OemNumber = dto.OemNumber,
            CrossReference = dto.CrossReference,
            DescriptionTr = dto.DescriptionTr,
            DescriptionEn = dto.DescriptionEn,
            SpecificationsTr = dto.SpecificationsTr,
            SpecificationsEn = dto.SpecificationsEn,
            TechnicalDetailsTr = dto.TechnicalDetailsTr,
            TechnicalDetailsEn = dto.TechnicalDetailsEn,
            BrandId = dto.BrandId,
            CategoryId = dto.CategoryId,
            Status = dto.Status,
            IsActive = dto.IsActive,
            IsFeatured = dto.IsFeatured,
            DisplayOrder = dto.DisplayOrder,
            MetaTitleTr = dto.MetaTitleTr,
            MetaTitleEn = dto.MetaTitleEn,
            MetaDescriptionTr = dto.MetaDescriptionTr,
            MetaDescriptionEn = dto.MetaDescriptionEn
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(ProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.Id)
            ?? throw new Exception("Product not found");

        product.NameTr = dto.NameTr;
        product.NameEn = dto.NameEn;
        product.Slug = dto.Slug;
        product.ProductCode = dto.ProductCode;
        product.OemNumber = dto.OemNumber;
        product.CrossReference = dto.CrossReference;
        product.DescriptionTr = dto.DescriptionTr;
        product.DescriptionEn = dto.DescriptionEn;
        product.SpecificationsTr = dto.SpecificationsTr;
        product.SpecificationsEn = dto.SpecificationsEn;
        product.TechnicalDetailsTr = dto.TechnicalDetailsTr;
        product.TechnicalDetailsEn = dto.TechnicalDetailsEn;
        product.BrandId = dto.BrandId;
        product.CategoryId = dto.CategoryId;
        product.Status = dto.Status;
        product.IsActive = dto.IsActive;
        product.IsFeatured = dto.IsFeatured;
        product.DisplayOrder = dto.DisplayOrder;
        product.MetaTitleTr = dto.MetaTitleTr;
        product.MetaTitleEn = dto.MetaTitleEn;
        product.MetaDescriptionTr = dto.MetaDescriptionTr;
        product.MetaDescriptionEn = dto.MetaDescriptionEn;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id)
            ?? throw new Exception("Product not found");

        // Soft delete
        product.IsActive = false;
        product.IsDeleted = true;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();
    }

    private ProductListDto MapProductListDto(Product product, string culture)
    {
        return new ProductListDto
        {
            Id = product.Id,
            Name = product.GetName(culture),
            NameTr = product.NameTr,
            NameEn = product.NameEn,
            Slug = product.Slug,
            ProductCode = product.ProductCode,
            BrandName = product.Brand?.GetName(culture) ?? "",
            BrandLogo = product.Brand?.LogoUrl,
            CategoryName = product.Category?.GetName(culture) ?? "",
            MainImageUrl = product.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
                          ?? product.Images?.FirstOrDefault()?.ImageUrl,
            IsActive = product.IsActive,
            IsFeatured = product.IsFeatured,
            IsNew = product.CreatedAt > DateTime.UtcNow.AddDays(-30)
        };
    }

    private ProductDto MapProductDto(Product product, string culture)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.GetName(culture),
            NameTr = product.NameTr,
            NameEn = product.NameEn,
            Slug = product.Slug,
            ProductCode = product.ProductCode,
            OemNumber = product.OemNumber,
            CrossReference = product.CrossReference,
            Description = product.GetDescription(culture),
            DescriptionTr = product.DescriptionTr,
            DescriptionEn = product.DescriptionEn,
            Specifications = culture == "en" ? product.SpecificationsEn : product.SpecificationsTr,
            SpecificationsTr = product.SpecificationsTr,
            SpecificationsEn = product.SpecificationsEn,
            TechnicalDetails = culture == "en" ? product.TechnicalDetailsEn : product.TechnicalDetailsTr,
            TechnicalDetailsTr = product.TechnicalDetailsTr,
            TechnicalDetailsEn = product.TechnicalDetailsEn,
            BrandId = product.BrandId,
            BrandName = product.Brand?.GetName(culture) ?? "",
            BrandSlug = product.Brand?.Slug,
            BrandLogo = product.Brand?.LogoUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.GetName(culture) ?? "",
            CategorySlug = product.Category?.Slug,
            Status = product.Status,
            IsActive = product.IsActive,
            IsFeatured = product.IsFeatured,
            IsNew = product.CreatedAt > DateTime.UtcNow.AddDays(-30),
            ViewCount = product.ViewCount,
            DisplayOrder = product.DisplayOrder,
            MainImageUrl = product.Images?.FirstOrDefault(i => i.IsMain)?.ImageUrl
                          ?? product.Images?.FirstOrDefault()?.ImageUrl,
            MetaTitle = culture == "en" ? product.MetaTitleEn : product.MetaTitleTr,
            MetaTitleTr = product.MetaTitleTr,
            MetaTitleEn = product.MetaTitleEn,
            MetaDescription = culture == "en" ? product.MetaDescriptionEn : product.MetaDescriptionTr,
            MetaDescriptionTr = product.MetaDescriptionTr,
            MetaDescriptionEn = product.MetaDescriptionEn,
            Images = product.Images?.Select(i => new ProductImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                ThumbnailUrl = i.ThumbnailUrl,
                AltText = i.AltText,
                IsMain = i.IsMain
            }).ToList() ?? new List<ProductImageDto>(),
            Documents = product.Documents?.Select(d => new ProductDocumentDto
            {
                Id = d.Id,
                Title = d.GetTitle(culture),
                TitleTr = d.TitleTr,
                TitleEn = d.TitleEn,
                FileUrl = d.FileUrl,
                FileType = d.FileType,
                FileSize = d.FileSize,
                DownloadCount = d.DownloadCount
            }).ToList() ?? new List<ProductDocumentDto>()
        };
    }
}
