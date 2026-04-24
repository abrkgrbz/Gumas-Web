using Gumas.Domain.Entities;

namespace Gumas.Web.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Slider> Sliders { get; set; } = new List<Slider>();
    public IEnumerable<Brand> FeaturedBrands { get; set; } = new List<Brand>();
    public IEnumerable<Product> FeaturedProducts { get; set; } = new List<Product>();
    public IEnumerable<News> LatestNews { get; set; } = new List<News>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
}
