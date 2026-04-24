using Gumas.Domain.Entities.Base;
using Gumas.Domain.Enums;

namespace Gumas.Domain.Entities;

public class Slider : LocalizableEntity
{
    public string? TitleTr { get; set; }
    public string? TitleEn { get; set; }
    public string? SubtitleTr { get; set; }
    public string? SubtitleEn { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? MobileImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? ButtonTextTr { get; set; }
    public string? ButtonTextEn { get; set; }
    public string? ButtonUrl { get; set; }
    public SliderType Type { get; set; } = SliderType.Hero;
    public int DisplayOrder { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string GetTitle(string culture) => culture == "en" && !string.IsNullOrEmpty(TitleEn) ? TitleEn : TitleTr ?? string.Empty;
    public string? GetSubtitle(string culture) => culture == "en" && !string.IsNullOrEmpty(SubtitleEn) ? SubtitleEn : SubtitleTr;
    public string? GetButtonText(string culture) => culture == "en" && !string.IsNullOrEmpty(ButtonTextEn) ? ButtonTextEn : ButtonTextTr;
}
