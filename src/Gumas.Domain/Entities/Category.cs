using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class Category : LocalizableEntity
{
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? IconClass { get; set; }
    public int? ParentId { get; set; }
    public int DisplayOrder { get; set; }

    // SEO
    public string? MetaTitleTr { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaDescriptionTr { get; set; }
    public string? MetaDescriptionEn { get; set; }

    // Navigation
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
