namespace Gumas.Application.Services;

public interface ISettingService
{
    Task<string> GetSettingValueAsync(string key, string defaultValue = "");
    Task<Dictionary<string, string>> GetSettingsByGroupAsync(string groupName);
    Task<Dictionary<string, string>> GetAllSettingsAsync();
    Task SaveSettingAsync(string key, string valueTr, string? valueEn = null, string groupName = "General");
    Task SaveSettingsAsync(Dictionary<string, string> settings, string groupName = "General");
    void ClearCache();
}
