namespace Gumas.Application.ViewModels;

public class LogoSettingsViewModel
{
    public string SiteLogoLight { get; set; } = "/images/logo/gumas_logo_light.png";
    public string SiteLogoDark { get; set; } = "/images/logo/gumas_logo_dark.png";
    public string SiteFavicon { get; set; } = "/images/logo/gumas_logo_light.png";
    public string AdminLogo { get; set; } = "/images/logo/gumas_logo_dark.png";

    public List<LogoFileItem> ExistingLogos { get; set; } = new();
}

public class LogoFileItem
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsInUse { get; set; }
    public List<string> UsedAs { get; set; } = new();
}
