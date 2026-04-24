using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Gumas.Web.Extensions;

public static class StringExtensions
{
    public static string ToSlug(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Convert to lowercase
        text = text.ToLowerInvariant();

        // Turkish character replacements
        text = text.Replace("ı", "i")
                   .Replace("ğ", "g")
                   .Replace("ü", "u")
                   .Replace("ş", "s")
                   .Replace("ö", "o")
                   .Replace("ç", "c")
                   .Replace("İ", "i")
                   .Replace("Ğ", "g")
                   .Replace("Ü", "u")
                   .Replace("Ş", "s")
                   .Replace("Ö", "o")
                   .Replace("Ç", "c");

        // Remove diacritics
        text = RemoveDiacritics(text);

        // Replace spaces and invalid chars with hyphens
        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
        text = Regex.Replace(text, @"\s+", "-");
        text = Regex.Replace(text, @"-+", "-");

        // Trim hyphens from ends
        text = text.Trim('-');

        return text;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
