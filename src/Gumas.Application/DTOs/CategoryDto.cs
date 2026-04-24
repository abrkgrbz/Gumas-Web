namespace Gumas.Application.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? ImageUrl { get; set; }
    public string? IconClass { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public int ProductCount { get; set; }
    public List<CategoryDto> Children { get; set; } = new();

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaTitleTr { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaDescriptionTr { get; set; }
    public string? MetaDescriptionEn { get; set; }

    // Localization helper methods
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
    public string? GetDescription(string culture) => culture == "en" && !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn : DescriptionTr;
}

public class CategoryListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? IconClass { get; set; }
    public string? ImageUrl { get; set; }
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public int ProductCount { get; set; }

    // Localization helper methods
    public string GetName(string culture) => culture == "en" && !string.IsNullOrEmpty(NameEn) ? NameEn : NameTr;
    public string? GetDescription(string culture) => culture == "en" && !string.IsNullOrEmpty(DescriptionEn) ? DescriptionEn : DescriptionTr;
}

public class CategoryCreateDto
{
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? DescriptionTr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? ImageUrl { get; set; }
    public string? IconClass { get; set; }
    public int? ParentId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public string? MetaTitleTr { get; set; }
    public string? MetaTitleEn { get; set; }
    public string? MetaDescriptionTr { get; set; }
    public string? MetaDescriptionEn { get; set; }
}
