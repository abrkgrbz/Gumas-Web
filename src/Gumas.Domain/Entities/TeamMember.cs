using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class TeamMember : LocalizableEntity
{
    public string? TitleTr { get; set; }
    public string? TitleEn { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LinkedInUrl { get; set; }
    public int DisplayOrder { get; set; }

    public string? GetTitle(string culture) => culture == "en" ? TitleEn : TitleTr;
}
