using Gumas.Domain.Entities;
using Gumas.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Gumas.Application.Services;

public class SettingService : ISettingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;
    private const string AllSettingsCacheKey = "All_Settings_Map";

    public SettingService(IUnitOfWork unitOfWork, IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<string> GetSettingValueAsync(string key, string defaultValue = "")
    {
        var settingsMap = await GetAllSettingsAsync();
        if (settingsMap.TryGetValue(key, out var val) && !string.IsNullOrWhiteSpace(val))
        {
            // If value is a relative web asset path (starts with '/'), verify file existence on disk
            if (val.StartsWith("/"))
            {
                try
                {
                    var relativeDiskPath = val.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeDiskPath);
                    if (File.Exists(fullPath))
                    {
                        return val;
                    }
                }
                catch
                {
                    // Fallback to default if path checking fails
                }
            }
            else
            {
                return val;
            }
        }

        return defaultValue;
    }

    public async Task<Dictionary<string, string>> GetSettingsByGroupAsync(string groupName)
    {
        var settings = await _unitOfWork.Settings.GetAllAsync(s => s.GroupName == groupName);
        return settings.ToDictionary(s => s.Key, s => s.ValueTr ?? string.Empty);
    }

    public async Task<Dictionary<string, string>> GetAllSettingsAsync()
    {
        if (_cache.TryGetValue(AllSettingsCacheKey, out Dictionary<string, string>? cachedMap) && cachedMap != null)
        {
            return cachedMap;
        }

        var allSettings = await _unitOfWork.Settings.GetAllAsync();
        var map = allSettings.ToDictionary(s => s.Key, s => s.ValueTr ?? string.Empty, StringComparer.OrdinalIgnoreCase);

        _cache.Set(AllSettingsCacheKey, map, TimeSpan.FromMinutes(30));
        return map;
    }

    public async Task SaveSettingAsync(string key, string valueTr, string? valueEn = null, string groupName = "General")
    {
        var settings = await _unitOfWork.Settings.GetAllAsync(s => s.Key == key);
        var existing = settings.FirstOrDefault();

        if (existing != null)
        {
            existing.ValueTr = valueTr;
            if (valueEn != null) existing.ValueEn = valueEn;
            existing.GroupName = groupName;
            existing.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Settings.Update(existing);
        }
        else
        {
            await _unitOfWork.Settings.AddAsync(new Setting
            {
                Key = key,
                ValueTr = valueTr,
                ValueEn = valueEn ?? valueTr,
                GroupName = groupName,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _unitOfWork.SaveChangesAsync();
        ClearCache();
    }

    public async Task SaveSettingsAsync(Dictionary<string, string> settings, string groupName = "General")
    {
        foreach (var kvp in settings)
        {
            var key = kvp.Key;
            var val = kvp.Value;

            var existingList = await _unitOfWork.Settings.GetAllAsync(s => s.Key == key);
            var existing = existingList.FirstOrDefault();

            if (existing != null)
            {
                existing.ValueTr = val;
                existing.GroupName = groupName;
                existing.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Settings.Update(existing);
            }
            else
            {
                await _unitOfWork.Settings.AddAsync(new Setting
                {
                    Key = key,
                    ValueTr = val,
                    ValueEn = val,
                    GroupName = groupName,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _unitOfWork.SaveChangesAsync();
        ClearCache();
    }

    public void ClearCache()
    {
        _cache.Remove(AllSettingsCacheKey);
    }
}
