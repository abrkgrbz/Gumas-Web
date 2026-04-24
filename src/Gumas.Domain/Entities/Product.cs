using Gumas.Domain.Entities.Base;
using Gumas.Domain.Enums;

namespace Gumas.Domain.Entities;

public class Product : LocalizableEntity
{
    public string Slug { get; set; } = string.Empty;
    public string? ProductCode { get; set; }
    public string? OemNumber { get; set; }
    public string? CrossReference { get; set; }

    // Specifications
    public string? SpecificationsTr { get; set; }
    public string? SpecificationsEn { get; set; }
    public string? TechnicalDetailsTr { get; set; }
    public string? TechnicalDetailsEn { get; set; }

    // Relations
    public int BrandId { get; set; }
    public int CategoryId { get; set; }

    // Status & Display
    public ProductStatus Status { get; set; } = ProductStatus.Active;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }

    // SEO
    public string? MetaTitleTr { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaDescriptionTr { get; set; }
    public string? MetaDescriptionEn { get; set; }

    // Navigation
    public virtual Brand Brand { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public virtual ICollection<ProductDocument> Documents { get; set; } = new List<ProductDocument>();
}
