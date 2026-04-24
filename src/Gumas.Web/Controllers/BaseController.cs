using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Gumas.Web.Resources;

namespace Gumas.Web.Controllers;

public abstract class BaseController : Controller
{
    protected readonly IStringLocalizer<SharedResources> Localizer;

    protected BaseController(IStringLocalizer<SharedResources> localizer)
    {
        Localizer = localizer;
    }

    protected string CurrentCulture =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

    protected bool IsEnglish => CurrentCulture == "en";
    protected bool IsTurkish => CurrentCulture == "tr";

    protected string GetLocalizedUrl(string turkishUrl, string englishUrl)
    {
        return IsEnglish ? englishUrl : turkishUrl;
    }

    protected IActionResult SetLanguageAndRedirect(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            }
        );

        return LocalRedirect(returnUrl);
    }
}
