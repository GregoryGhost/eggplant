namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Lagalike.Demo.Eggplant.MVU.Models;
    using Lagalike.Demo.Eggplant.MVU.Services.Domain;
    using Lagalike.Demo.Eggplant.MVU.Services.ModuleSettings;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Extensions;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    ///     A cache of demo Test Patrick Star.
    /// </summary>
    public class CockSizerCache : BaseTelegramBotCache<Model>
    {
        /// <inheritdoc />
        public CockSizerCache(IMemoryCache telegramCache, CockSizerInfo modeInfo)
            : base(telegramCache, modeInfo.ModeInfo.Name)
        {
        }

        public IEnumerable<UserCockSize> GetCheckedUsers()
        {
            var checkedUsers = GetUsersIds()
                               .Select(
                                   userId =>
                                   {
                                       TryGetValue(userId, out var userCockSize);

                                       return (UserId: ParseUserId(userId), UserCockSize: userCockSize);
                                   })
                               .Where(x => x.UserCockSize?.CockSizeModel?.CockSize != null)
                               .Select(
                                   x =>
                                   {
                                       if (x.UserCockSize.CockSizeModel?.CockSize == null)
                                           throw new ArgumentNullException(
                                               nameof(x.UserCockSize.CockSizeModel),
                                               $"User cock size is null by {nameof(x.UserId)} {x.UserId}");

                                       if (x.UserId == null)
                                           throw new ArgumentNullException(nameof(x.UserId), "Cannot parse a user id");

                                       return new UserCockSize
                                       {
                                           UserId = x.UserId.Value,
                                           CockSize = x.UserCockSize.CockSizeModel.CockSize
                                       };
                                   });

            return checkedUsers;
        }

        private IEnumerable<string> GetUsersIds()
        {
            return _telegramCache.GetKeys<string>()
                                 .Select(x => x[(_demoCacheName.Length + 1)..]);
        }

        private static long? ParseUserId(string userId)
        {
            return long.TryParse(userId, out var parsedUserId)
                ? parsedUserId
                : null;
        }
    }

    public record UserCockSize
    {
        public CockSize CockSize { get; init; } = null!;

        public long UserId { get; init; }
    }
}