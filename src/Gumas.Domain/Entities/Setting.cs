using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class Setting : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string? ValueTr { get; set; }
    public string? ValueEn { get; set; }
    public string? GroupName { get; set; }

    public string? GetValue(string culture) => culture == "en" ? ValueEn : ValueTr;
}
