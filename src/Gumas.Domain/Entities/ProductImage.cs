using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsMain { get; set; }

    // Navigation
    public virtual Product Product { get; set; } = null!;
}
