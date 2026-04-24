namespace Gumas.Domain.Entities.Base;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}

public abstract class LocalizableEntity : BaseEntity
{
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }

    public string GetName(string culture) => culture == "en" ? NameEn : NameTr;
    public string? GetDescription(string culture) => culture == "en" ? DescriptionEn : DescriptionTr;
}
