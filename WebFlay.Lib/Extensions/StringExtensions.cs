namespace WebFlay.Lib.Extensions;

public static class StringExtensions
{
    public static Uri? CreateUri(this string url)
    {
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            url = $"http://{url}";
        }

        try
        {
            return new Uri(url);
        }
        catch (UriFormatException)
        {
            return null;
        }
    }


    public static string Join<T>(this IEnumerable<T> source, string separator = ", ")
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return string.Join(separator, source.Select(item => item?.ToString() ?? string.Empty));
    }
}