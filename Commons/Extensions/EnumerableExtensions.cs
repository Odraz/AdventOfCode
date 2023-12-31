public static class EnumerableExtensions
{
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
    {
        return source.Skip(Math.Max(0, source.Count() - n));
    }
}