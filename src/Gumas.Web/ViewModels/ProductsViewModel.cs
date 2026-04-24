using Gumas.Domain.Entities;

namespace Gumas.Web.ViewModels;

public class ProductsViewModel
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public IEnumerable<Brand> Brands { get; set; } = new List<Brand>();
    public Dictionary<int, int> CategoryProductCounts { get; set; } = new();
    public int? SelectedCategoryId { get; set; }
    public int? SelectedBrandId { get; set; }
    public string? SearchTerm { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class ProductDetailViewModel
{
    public Product Product { get; set; } = null!;
    public IEnumerable<Product> RelatedProducts { get; set; } = new List<Product>();
}
