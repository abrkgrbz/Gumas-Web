using Gumas.Application.DTOs;

namespace Gumas.Application.ViewModels;

public class HomeViewModel
{
    public List<SliderDto> Sliders { get; set; } = new();
    public List<BrandListDto> FeaturedBrands { get; set; } = new();
    public List<ProductListDto> FeaturedProducts { get; set; } = new();
    public List<CategoryListDto> Categories { get; set; } = new();
    public List<NewsListDto> LatestNews { get; set; } = new();
    public Dictionary<string, string> Statistics { get; set; } = new();
}

public class ProductListViewModel
{
    public List<ProductListDto> Products { get; set; } = new();
    public List<BrandListDto> Brands { get; set; } = new();
    public List<CategoryListDto> Categories { get; set; } = new();
    public ProductFilterDto Filter { get; set; } = new();

    // Filter state
    public string? SearchTerm { get; set; }
    public string? SelectedCategory { get; set; }
    public string? SelectedBrand { get; set; }
    public string? SortBy { get; set; }

    // Pagination
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = 12;
}

public class ProductDetailViewModel
{
    public ProductDto Product { get; set; } = null!;
    public List<ProductListDto> RelatedProducts { get; set; } = new();
    public BrandDto Brand { get; set; } = null!;
}

public class BrandDetailViewModel
{
    public BrandDto Brand { get; set; } = null!;
    public List<ProductListDto> Products { get; set; } = new();
    public List<CategoryListDto> Categories { get; set; } = new();
    public List<BrandListDto> OtherBrands { get; set; } = new();
    public int TotalProducts { get; set; }
    public int ProductCount { get; set; }
}

public class ContactViewModel
{
    public ContactMessageDto Message { get; set; } = new();
    public Dictionary<string, string> Settings { get; set; } = new();
    public List<ProductListDto>? Products { get; set; }
}

public class CorporateViewModel
{
    public List<TeamMemberDto> TeamMembers { get; set; } = new();
    public List<CertificateDto> Certificates { get; set; } = new();
    public Dictionary<string, string> Settings { get; set; } = new();
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
