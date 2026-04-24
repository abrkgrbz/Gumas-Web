using Gumas.Domain.Enums;

namespace Gumas.Application.DTOs;

public class SliderDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? TitleTr { get; set; }
    public string? TitleEn { get; set; }
    public string? Subtitle { get; set; }
    public string? SubtitleTr { get; set; }
    public string? SubtitleEn { get; set; }
    public string? Description { get; set; }
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? MobileImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonTextTr { get; set; }
    public string? ButtonTextEn { get; set; }
    public string? ButtonUrl { get; set; }
    public SliderType Type { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;

    // Localization helper methods
    public string GetTitle(string culture) => culture == "en" && !string.IsNullOrEmpty(TitleEn) ? TitleEn : TitleTr ?? string.Empty;
    public string GetSubtitle(string culture) => culture == "en" && !string.IsNullOrEmpty(SubtitleEn) ? SubtitleEn : SubtitleTr ?? string.Empty;
    public string? GetDescription(string culture) => culture == "en" && !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn : DescriptionTr;
    public string? GetButtonText(string culture) => culture == "en" && !string.IsNullOrEmpty(ButtonTextEn) ? ButtonTextEn : ButtonTextTr;
}

public class NewsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ContentTr { get; set; }
    public string? ContentEn { get; set; }
    public string? Summary { get; set; }
    public string? SummaryTr { get; set; }
    public string? SummaryEn { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DateTime PublishDate { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public int ViewCount { get; set; }

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    // Localization helper methods
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
    public string? GetSummary(string culture) => culture == "en" && !string.IsNullOrEmpty(SummaryEn) ? SummaryEn : SummaryTr;
    public string? GetContent(string culture) => culture == "en" && !string.IsNullOrEmpty(ContentEn) ? ContentEn : ContentTr;
}

public class NewsListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? SummaryTr { get; set; }
    public string? SummaryEn { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DateTime PublishDate { get; set; }

    // Localization helper methods
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
    public string GetTitle(string culture) => GetName(culture);
    public string? GetSummary(string culture) => culture == "en" && !string.IsNullOrEmpty(SummaryEn) ? SummaryEn : SummaryTr;
}

public class ContactMessageDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? CompanyName { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public MessageStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SettingDto
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? GroupName { get; set; }
}

public class CertificateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? IssuingOrganization { get; set; }
    public DateTime? IssueDate { get; set; }

    // Localization helper methods
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
    public string? GetDescription(string culture) => culture == "en" && !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn : DescriptionTr;
}

public class TeamMemberDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? TitleTr { get; set; }
    public string? TitleEn { get; set; }
    public string? Description { get; set; }
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LinkedInUrl { get; set; }

    // Localization helper methods
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
    public string? GetTitle(string culture) => culture == "en" && !string.IsNullOrEmpty(TitleEn) ? TitleEn : TitleTr;
    public string? GetDescription(string culture) => culture == "en" && !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn : DescriptionTr;
}
