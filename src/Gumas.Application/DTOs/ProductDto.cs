using System.ComponentModel.DataAnnotations;
using Gumas.Domain.Enums;

namespace Gumas.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ProductCode { get; set; }
    public string? OemNumber { get; set; }
    public string? CrossReference { get; set; }
    public string? Description { get; set; }
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? Specifications { get; set; }
    public string? SpecificationsTr { get; set; }
    public string? SpecificationsEn { get; set; }
    public string? TechnicalDetails { get; set; }
    public string? TechnicalDetailsTr { get; set; }
    public string? TechnicalDetailsEn { get; set; }

    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string? BrandSlug { get; set; }
    public string? BrandLogo { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? CategorySlug { get; set; }

    public ProductStatus Status { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public bool IsNew { get; set; }
    public int ViewCount { get; set; }
    public int DisplayOrder { get; set; }

    public string? MainImageUrl { get; set; }
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductDocumentDto> Documents { get; set; } = new();

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaTitleTr { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaDescriptionTr { get; set; }
    public string? MetaDescriptionEn { get; set; }

    // Localization helper methods
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
    public string? GetDescription(string culture) => culture == "en" && !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn : DescriptionTr;
    public string? GetSpecifications(string culture) => culture == "en" && !string.IsNullOrEmpty(SpecificationsEn) ? SpecificationsEn : SpecificationsTr;
}

public class ProductListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ProductCode { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string? BrandLogo { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? MainImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public bool IsNew { get; set; }

    // Localization helper method
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
}

public class ProductImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public bool IsMain { get; set; }
}

public class ProductDocumentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TitleTr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string? FileType { get; set; }
    public long? FileSize { get; set; }
    public int DownloadCount { get; set; }

    public string GetTitle(string culture) => culture == "en" && !string.IsNullOrEmpty(TitleEn) ? TitleEn : TitleTr;
}

public class ProductCreateDto
{
    [Required(ErrorMessage = "Türkçe ürün adı zorunludur.")]
    [StringLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir.")]
    public string NameTr { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir.")]
    public string NameEn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug zorunludur.")]
    [StringLength(200, ErrorMessage = "Slug en fazla 200 karakter olabilir.")]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug sadece küçük harf, rakam ve tire içerebilir.")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Ürün kodu en fazla 50 karakter olabilir.")]
    public string? ProductCode { get; set; }

    [StringLength(50, ErrorMessage = "OEM numarası en fazla 50 karakter olabilir.")]
    public string? OemNumber { get; set; }

    [StringLength(500, ErrorMessage = "Çapraz referans en fazla 500 karakter olabilir.")]
    public string? CrossReference { get; set; }

    [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
    public string? DescriptionTr { get; set; }

    [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
    public string? DescriptionEn { get; set; }

    public string? SpecificationsTr { get; set; }
    public string? SpecificationsEn { get; set; }
    public string? TechnicalDetailsTr { get; set; }
    public string? TechnicalDetailsEn { get; set; }

    [Required(ErrorMessage = "Marka seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir marka seçiniz.")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçiniz.")]
    public int CategoryId { get; set; }

    public ProductStatus Status { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public bool IsNew { get; set; }

    [Range(0, 9999, ErrorMessage = "Sıralama değeri 0-9999 arasında olmalıdır.")]
    public int DisplayOrder { get; set; }

    [StringLength(70, ErrorMessage = "Meta başlık en fazla 70 karakter olabilir.")]
    public string? MetaTitleTr { get; set; }

    [StringLength(70, ErrorMessage = "Meta başlık en fazla 70 karakter olabilir.")]
    public string? MetaTitleEn { get; set; }

    [StringLength(160, ErrorMessage = "Meta açıklama en fazla 160 karakter olabilir.")]
    public string? MetaDescriptionTr { get; set; }

    [StringLength(160, ErrorMessage = "Meta açıklama en fazla 160 karakter olabilir.")]
    public string? MetaDescriptionEn { get; set; }
}

public class ProductFilterDto
{
    public int? BrandId { get; set; }
    public int? CategoryId { get; set; }
    public string? SearchTerm { get; set; }
    public bool? IsFeatured { get; set; }
    public ProductStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
}
