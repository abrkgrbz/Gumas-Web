using System.Globalization;

namespace Gumas.Web.Extensions;

public static class CultureExtensions
{
    public static string GetCurrentCulture(this HttpContext context)
    {
        return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    }

    public static bool IsEnglish(this HttpContext context)
    {
        return context.GetCurrentCulture() == "en";
    }

    public static bool IsTurkish(this HttpContext context)
    {
        return context.GetCurrentCulture() == "tr";
    }

    public static string GetLocalizedUrl(this HttpContext context, string turkishUrl, string englishUrl)
    {
        return context.IsEnglish() ? englishUrl : turkishUrl;
    }
}
