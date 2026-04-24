using Gumas.Domain.Entities;

namespace Gumas.Web.ViewModels;

public class BrandsViewModel
{
    public IEnumerable<Brand> Brands { get; set; } = new List<Brand>();
    public int TotalCount { get; set; }
}

public class BrandDetailViewModel
{
    public Brand Brand { get; set; } = null!;
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public int TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
