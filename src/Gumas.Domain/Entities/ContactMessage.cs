using Gumas.Domain.Entities.Base;
using Gumas.Domain.Enums;

namespace Gumas.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public MessageStatus Status { get; set; } = MessageStatus.New;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? AdminNotes { get; set; }

    // Quote request specific
    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
}
