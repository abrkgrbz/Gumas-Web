using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class Certificate : LocalizableEntity
{
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? IssuingOrganization { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int DisplayOrder { get; set; }
}
