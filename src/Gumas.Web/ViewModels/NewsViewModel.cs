using Gumas.Domain.Entities;

namespace Gumas.Web.ViewModels;

public class NewsListViewModel
{
    public IEnumerable<News> News { get; set; } = new List<News>();
    public int TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class NewsDetailViewModel
{
    public News News { get; set; } = null!;
    public IEnumerable<News> RelatedNews { get; set; } = new List<News>();
}
