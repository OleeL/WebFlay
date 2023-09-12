
namespace WebFlay.Lib.Extensions;

public static class Helpers
{
    public static IEnumerable<(T Node, int Index)> Index<T>(this IEnumerable<T> iterable)
    {
        int index = 0;
        foreach (var item in iterable)
        {
            yield return (item, index);
            index++;
        }
    }
}