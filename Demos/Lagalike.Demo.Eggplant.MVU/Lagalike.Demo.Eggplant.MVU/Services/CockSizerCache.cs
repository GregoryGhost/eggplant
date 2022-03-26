namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// A cache of demo Test Patrick Star.
    /// </summary>
    public class CockSizerCache : BaseTelegramBotCache<Model>
    {
        /// <inheritdoc />
        public CockSizerCache(IMemoryCache telegramCache, CockSizerInfo modeInfo)
            : base(telegramCache, modeInfo.ModeInfo.Name)
        {
        }
    }
}