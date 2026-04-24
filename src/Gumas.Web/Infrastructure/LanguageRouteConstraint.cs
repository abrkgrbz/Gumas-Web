namespace Gumas.Web.Infrastructure;

public class LanguageRouteConstraint : IRouteConstraint
{
    private readonly HashSet<string> _validLanguages = new(StringComparer.OrdinalIgnoreCase) { "tr", "en" };

    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var value))
            return false;

        var language = value?.ToString()?.ToLowerInvariant();
        return _validLanguages.Contains(language ?? string.Empty);
    }
}
