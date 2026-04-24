using Gumas.Application.ViewModels;

namespace Gumas.Application.Services;

public interface IActivityLogService
{
    Task LogActivityAsync(string action, string entityType, int? entityId, string entityName, string details, string userName, string ipAddress);
    Task<PagedResult<ActivityLogDto>> GetActivityLogsAsync(int page, int pageSize, string? entityType = null, string? action = null);
    Task<List<ActivityLogDto>> GetRecentActivitiesAsync(int count = 10);
}

public class ActivityLogDto
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public string ActionIcon => Action switch
    {
        "Create" => "bi-plus-circle text-success",
        "Update" => "bi-pencil text-warning",
        "Delete" => "bi-trash text-danger",
        "View" => "bi-eye text-info",
        "Export" => "bi-download text-primary",
        "Import" => "bi-upload text-primary",
        "Login" => "bi-box-arrow-in-right text-success",
        "Logout" => "bi-box-arrow-right text-secondary",
        _ => "bi-activity text-muted"
    };

    public string ActionText => Action switch
    {
        "Create" => "Olusturuldu",
        "Update" => "Guncellendi",
        "Delete" => "Silindi",
        "View" => "Goruntulendi",
        "Export" => "Disari Aktarildi",
        "Import" => "Iceri Aktarildi",
        "Login" => "Giris Yapildi",
        "Logout" => "Cikis Yapildi",
        _ => Action
    };

    public string EntityTypeText => EntityType switch
    {
        "Product" => "Urun",
        "Brand" => "Marka",
        "Category" => "Kategori",
        "ContactMessage" => "Iletisim Mesaji",
        "News" => "Haber",
        "Slider" => "Slider",
        "Certificate" => "Sertifika",
        "TeamMember" => "Ekip Uyesi",
        _ => EntityType
    };
}
