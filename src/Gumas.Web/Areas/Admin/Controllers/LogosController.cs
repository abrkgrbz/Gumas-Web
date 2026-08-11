using Gumas.Application.Services;
using Gumas.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gumas.Web.Areas.Admin.Controllers;

public class LogosController : BaseAdminController
{
    private readonly ISettingService _settingService;
    private readonly IWebHostEnvironment _env;
    private readonly string[] _allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".svg", ".webp", ".ico" };

    public LogosController(ISettingService settingService, IWebHostEnvironment env)
    {
        _settingService = settingService;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var model = new LogoSettingsViewModel
        {
            SiteLogoLight = await _settingService.GetSettingValueAsync("SiteLogoLight", "/images/logo/gumas_logo_light.png"),
            SiteLogoDark = await _settingService.GetSettingValueAsync("SiteLogoDark", "/images/logo/gumas_logo_dark.png"),
            SiteFavicon = await _settingService.GetSettingValueAsync("SiteFavicon", "/images/logo/gumas_logo_light.png"),
            AdminLogo = await _settingService.GetSettingValueAsync("AdminLogo", "/images/logo/gumas_logo_dark.png"),
            ExistingLogos = GetExistingLogoFiles()
        };

        // Mark active usage
        foreach (var logoFile in model.ExistingLogos)
        {
            if (string.Equals(logoFile.FilePath, model.SiteLogoLight, StringComparison.OrdinalIgnoreCase))
            {
                logoFile.IsInUse = true;
                logoFile.UsedAs.Add("Aydınlık Tema Logosu");
            }
            if (string.Equals(logoFile.FilePath, model.SiteLogoDark, StringComparison.OrdinalIgnoreCase))
            {
                logoFile.IsInUse = true;
                logoFile.UsedAs.Add("Karanlık Tema Logosu");
            }
            if (string.Equals(logoFile.FilePath, model.SiteFavicon, StringComparison.OrdinalIgnoreCase))
            {
                logoFile.IsInUse = true;
                logoFile.UsedAs.Add("Favicon");
            }
            if (string.Equals(logoFile.FilePath, model.AdminLogo, StringComparison.OrdinalIgnoreCase))
            {
                logoFile.IsInUse = true;
                logoFile.UsedAs.Add("Admin Panel Logosu");
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadLogo(IFormFile? file, string logoType)
    {
        if (!IsValidLogoType(logoType))
        {
            TempData["Error"] = "Geçersiz logo hedef alanı.";
            return RedirectToAction(nameof(Index));
        }

        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Lütfen bir logo dosyası seçin.";
            return RedirectToAction(nameof(Index));
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext))
        {
            TempData["Error"] = $"Geçersiz dosya formatı ({ext}). İzin verilen formatlar: .png, .jpg, .jpeg, .svg, .webp, .ico";
            return RedirectToAction(nameof(Index));
        }

        if (file.Length > 10 * 1024 * 1024)
        {
            TempData["Error"] = "Logo dosya boyutu maksimum 10MB olabilir.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "logos");
            Directory.CreateDirectory(uploadsFolder);

            var safeFileName = Path.GetFileNameWithoutExtension(file.FileName)
                .Replace(" ", "_")
                .Replace("-", "_");
            
            // Clean invalid chars
            safeFileName = string.Concat(safeFileName.Split(Path.GetInvalidFileNameChars()));
            if (string.IsNullOrWhiteSpace(safeFileName)) safeFileName = "logo";

            var uniqueFileName = $"{safeFileName}_{DateTime.UtcNow:yyyyMMddHHmmss}{ext}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/logos/{uniqueFileName}";
            await _settingService.SaveSettingAsync(logoType, relativePath, relativePath, "Logos");

            TempData["Success"] = $"{GetLogoTypeName(logoType)} başarıyla yüklendi ve güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Logo yüklenirken bir hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SelectLogo(string logoType, string logoPath)
    {
        if (!IsValidLogoType(logoType))
        {
            TempData["Error"] = "Geçersiz logo hedef alanı.";
            return RedirectToAction(nameof(Index));
        }

        if (string.IsNullOrWhiteSpace(logoPath))
        {
            TempData["Error"] = "Geçersiz logo yolu.";
            return RedirectToAction(nameof(Index));
        }

        await _settingService.SaveSettingAsync(logoType, logoPath, logoPath, "Logos");
        TempData["Success"] = $"{GetLogoTypeName(logoType)} başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetDefault(string logoType)
    {
        if (!IsValidLogoType(logoType))
        {
            TempData["Error"] = "Geçersiz logo hedef alanı.";
            return RedirectToAction(nameof(Index));
        }

        string defaultValue = logoType switch
        {
            "SiteLogoLight" => "/images/logo/gumas_logo_light.png",
            "SiteLogoDark" => "/images/logo/gumas_logo_dark.png",
            "SiteFavicon" => "/images/logo/gumas_logo_light.png",
            "AdminLogo" => "/images/logo/gumas_logo_dark.png",
            _ => "/images/logo/gumas_logo_light.png"
        };

        await _settingService.SaveSettingAsync(logoType, defaultValue, defaultValue, "Logos");
        TempData["Success"] = $"{GetLogoTypeName(logoType)} varsayılan logoya sıfırlandı.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLogo(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            TempData["Error"] = "Geçersiz dosya yolu.";
            return RedirectToAction(nameof(Index));
        }

        // Prevent deleting original system logo defaults
        if (filePath.StartsWith("/images/logo/gumas_logo", StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "Sistem varsayılan logosu silinemez.";
            return RedirectToAction(nameof(Index));
        }

        var light = await _settingService.GetSettingValueAsync("SiteLogoLight");
        var dark = await _settingService.GetSettingValueAsync("SiteLogoDark");
        var fav = await _settingService.GetSettingValueAsync("SiteFavicon");
        var admin = await _settingService.GetSettingValueAsync("AdminLogo");

        if (string.Equals(filePath, light, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(filePath, dark, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(filePath, fav, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(filePath, admin, StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "Aktif kullanımda olan logo dosyası silinemez. Lütfen önce başka bir logo seçin.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var absolutePath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(absolutePath))
            {
                System.IO.File.Delete(absolutePath);
                TempData["Success"] = "Logo dosyası başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = "Dosya sunucuda bulunamadı.";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Dosya silinirken hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    private List<LogoFileItem> GetExistingLogoFiles()
    {
        var list = new List<LogoFileItem>();
        var directoriesToScan = new[]
        {
            Path.Combine(_env.WebRootPath, "uploads", "logos"),
            Path.Combine(_env.WebRootPath, "images", "logo")
        };

        foreach (var dir in directoriesToScan)
        {
            if (!Directory.Exists(dir)) continue;

            var dirInfo = new DirectoryInfo(dir);
            var files = dirInfo.GetFiles()
                .Where(f => _allowedExtensions.Contains(f.Extension.ToLowerInvariant()))
                .OrderByDescending(f => f.LastWriteTimeUtc);

            foreach (var file in files)
            {
                var relativePath = "/" + Path.GetRelativePath(_env.WebRootPath, file.FullName).Replace('\\', '/');
                list.Add(new LogoFileItem
                {
                    FileName = file.Name,
                    FilePath = relativePath,
                    SizeBytes = file.Length,
                    LastModified = file.LastWriteTime
                });
            }
        }

        return list;
    }

    private static bool IsValidLogoType(string logoType)
    {
        return logoType is "SiteLogoLight" or "SiteLogoDark" or "SiteFavicon" or "AdminLogo";
    }

    private static string GetLogoTypeName(string logoType)
    {
        return logoType switch
        {
            "SiteLogoLight" => "Aydınlık Tema Logosu",
            "SiteLogoDark" => "Karanlık Tema Logosu",
            "SiteFavicon" => "Favicon (Tarayıcı İkonu)",
            "AdminLogo" => "Admin Panel Logosu",
            _ => "Logo"
        };
    }
}
