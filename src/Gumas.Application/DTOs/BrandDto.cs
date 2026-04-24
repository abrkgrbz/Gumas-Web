using System.ComponentModel.DataAnnotations;

namespace Gumas.Application.DTOs;

public class BrandDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? LongDescription { get; set; }
    public string? LongDescriptionTr { get; set; }
    public string? LongDescriptionEn { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Website { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Country { get; set; }
    public int? FoundedYear { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }
    public int ProductCount { get; set; }

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
    public string? GetLongDescription(string culture) => culture == "en" && !string.IsNullOrEmpty(LongDescriptionEn) ? LongDescriptionEn : LongDescriptionTr;
}

public class BrandListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public int ProductCount { get; set; }
    public bool IsActive { get; set; } = true;

    // Localization helper method
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
}

public class BrandCreateDto
{
    [Required(ErrorMessage = "Türkçe marka adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Marka adı en fazla 100 karakter olabilir.")]
    public string NameTr { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Marka adı en fazla 100 karakter olabilir.")]
    public string NameEn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug zorunludur.")]
    [StringLength(100, ErrorMessage = "Slug en fazla 100 karakter olabilir.")]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug sadece küçük harf, rakam ve tire içerebilir.")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? DescriptionTr { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? DescriptionEn { get; set; }

    public string? LongDescriptionTr { get; set; }
    public string? LongDescriptionEn { get; set; }

    [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
    public string? LogoUrl { get; set; }

    [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
    public string? BannerUrl { get; set; }

    [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
    public string? Website { get; set; }

    [StringLength(50, ErrorMessage = "Ülke en fazla 50 karakter olabilir.")]
    public string? Country { get; set; }

    [Range(1800, 2100, ErrorMessage = "Kuruluş yılı 1800-2100 arasında olmalıdır.")]
    public int? FoundedYear { get; set; }

    [Range(0, 9999, ErrorMessage = "Sıralama değeri 0-9999 arasında olmalıdır.")]
    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }

    [StringLength(70, ErrorMessage = "Meta başlık en fazla 70 karakter olabilir.")]
    public string? MetaTitleTr { get; set; }

    [StringLength(70, ErrorMessage = "Meta başlık en fazla 70 karakter olabilir.")]
    public string? MetaTitleEn { get; set; }

    [StringLength(160, ErrorMessage = "Meta açıklama en fazla 160 karakter olabilir.")]
    public string? MetaDescriptionTr { get; set; }

    [StringLength(160, ErrorMessage = "Meta açıklama en fazla 160 karakter olabilir.")]
    public string? MetaDescriptionEn { get; set; }
}
