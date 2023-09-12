using System.Text.RegularExpressions;

namespace WebFlay.Lib.Extensions;

public static class UriExtensions
{
    public static string? GetMainDomain(this Uri uri)
    {
        var topLevelDomains = new List<string>
        {
            "co.uk",
            "com.au",
            "co.nz",
            "co.za",
            "com.br",
            "co.jp",
            "com.ar",
            "com.mx",
            "co.in"
        };

        var host = uri.Host;
        string? hostWithoutPrefix =
            (from tld in topLevelDomains
                select new Regex($"(?<=\\.|)\\w+\\.{tld}$")
                into regex
                select regex.Match(host)
                into match
                where match.Success
                select match.Groups[0].Value).FirstOrDefault();

        if (string.IsNullOrWhiteSpace(hostWithoutPrefix))
        {
            var regex = new Regex("(?<=\\.|)\\w+\\.\\w+$");
            var match = regex.Match(host);

            if (match.Success)
                hostWithoutPrefix = match.Groups[0].Value;
        }

        return hostWithoutPrefix;
    }
}