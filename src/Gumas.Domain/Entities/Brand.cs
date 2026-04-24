using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class Brand : LocalizableEntity
{
    public string Slug { get; set; } = string.Empty;
    public string? LongDescriptionTr { get; set; }
    public string? LongDescriptionEn { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Website { get; set; }
    public string? Country { get; set; }
    public int? FoundedYear { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }

    // SEO
    public string? MetaTitleTr { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaDescriptionTr { get; set; }
    public string? MetaDescriptionEn { get; set; }

    // Navigation
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    // Localization helper for long description
    public string? GetLongDescription(string culture) =>
        culture == "en" && !string.IsNullOrEmpty(LongDescriptionEn) ? LongDescriptionEn : LongDescriptionTr;
}
