using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class ProductDocument : BaseEntity
{
    public int ProductId { get; set; }
    public string TitleTr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string? FileType { get; set; } // PDF, DOC, etc.
    public long? FileSize { get; set; }
    public int DisplayOrder { get; set; }
    public int DownloadCount { get; set; }

    public string GetTitle(string culture) => culture == "en" ? TitleEn : TitleTr;

    // Navigation
    public virtual Product Product { get; set; } = null!;
}
