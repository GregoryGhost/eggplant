namespace Lagalike.Telegram.Shared.Contracts
{
    using System;

    using global::PatrickStar.MVU;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    ///     A basic implementation of cache for demos of a telegram bot.
    /// </summary>
    /// <typeparam name="TItem">A type of a saved item.</typeparam>
    public abstract class BaseTelegramBotCache<TItem> : IDisposable, IModelCache<TItem>
    {
        protected readonly string DemoCacheName;

        protected readonly IMemoryCache TelegramCache;

        /// <summary>
        ///     Initialize dependencies.
        /// </summary>
        /// <param name="telegramCache">A memory cache for the Telegram.</param>
        /// <param name="demoCacheName">A cache name of demo mode.</param>
        protected BaseTelegramBotCache(IMemoryCache telegramCache, string demoCacheName)
        {
            TelegramCache = telegramCache;
            DemoCacheName = demoCacheName;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            TelegramCache.Dispose();
        }

        /// <summary>
        ///     Remove a record by the Telegram chat id.
        /// </summary>
        /// <param name="chatId">A chat id.</param>
        public void Remove(string chatId)
        {
            TelegramCache.Remove(FormatCacheKey(chatId));
        }

        /// <inheritdoc />
        public void Set(string chatId, TItem value)
        {
            TelegramCache.Set(FormatCacheKey(chatId), value);
        }

        /// <inheritdoc />
        public bool TryGetValue(string chatId, out TItem value)
        {
            return TelegramCache.TryGetValue(FormatCacheKey(chatId), out value);
        }

        private string FormatCacheKey(string chatId)
        {
            return $"{DemoCacheName}_{chatId}";
        }
    }
}