using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class News : LocalizableEntity
{
    public string Slug { get; set; } = string.Empty;
    public string? ContentTr { get; set; }
    public string? ContentEn { get; set; }
    public string? SummaryTr { get; set; }
    public string? SummaryEn { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DateTime PublishDate { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }

    // SEO
    public string? MetaTitleTr { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaDescriptionTr { get; set; }
    public string? MetaDescriptionEn { get; set; }

    public string? GetContent(string culture) => culture == "en" ? ContentEn : ContentTr;
    public string? GetSummary(string culture) => culture == "en" ? SummaryEn : SummaryTr;

    // Alias for GetName - for semantic clarity in news context
    public string GetTitle(string culture) => GetName(culture);
}
