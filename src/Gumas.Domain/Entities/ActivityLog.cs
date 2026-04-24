using Gumas.Domain.Entities.Base;

namespace Gumas.Domain.Entities;

public class ActivityLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public enum ActivityAction
{
    Create,
    Update,
    Delete,
    View,
    Export,
    Import,
    Login,
    Logout
}
