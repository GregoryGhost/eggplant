namespace Lagalike.Telegram.Shared.Extensions
{
    public static class MemoryCacheExtensions
    {
        private static readonly Func<MemoryCache, object> GetEntriesCollection;

        static MemoryCacheExtensions()
        {
            var entriesCollectionMethod = typeof(MemoryCache)
                                          .GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)
                                          ?.GetGetMethod(true);

            if (entriesCollectionMethod is null)
                throw new ArgumentNullException(nameof(entriesCollectionMethod));

            var sourceDelegate = Delegate.CreateDelegate(
                typeof(Func<MemoryCache, object>),
                entriesCollectionMethod,
                true);
            if (sourceDelegate is not Func<MemoryCache, object> entriesCollectionDelegate)
                throw new ArgumentNullException(nameof(entriesCollectionDelegate));

            GetEntriesCollection = entriesCollectionDelegate;
        }

        public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache)
        {
            return GetKeys(memoryCache).OfType<T>();
        }

        public static IEnumerable<TValue> GetValues<TValue>(this IMemoryCache memoryCache)
        {
            return GetValues(memoryCache)
                   .OfType<object>()
                   .Select(x => (TValue?)x.GetValue("Value"))
                   .Where(x => x is not null)!;
        }

        private static IEnumerable GetKeys(this IMemoryCache memoryCache)
        {
            return ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;
        }

        private static object? GetValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }

        private static IEnumerable GetValues(this IMemoryCache memoryCache)
        {
            return ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Values;
        }
    }
}