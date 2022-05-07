namespace Lagalike.Telegram.Shared.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.Caching.Memory;

    public static class MemoryCacheExtensions
    {
        private static readonly Func<MemoryCache, object> GetEntriesCollection;

        static MemoryCacheExtensions()
        {
            var entriesCollectionMethod = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)
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
        
        public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
            GetKeys(memoryCache).OfType<T>();
        
        private static IEnumerable GetKeys(this IMemoryCache memoryCache) =>
            ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;
    }
}