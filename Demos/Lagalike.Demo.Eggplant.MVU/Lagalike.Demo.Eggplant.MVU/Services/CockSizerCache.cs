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
    /// A cache of demo Test Patrick Star.
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
            var userIds = _telegramCache.GetKeys<string>();
            var checkedUsers = userIds.Select(
                                          userId =>
                                          {
                                              TryGetValue(userId, out var userCockSize);

                                              return (UserId: ParseUserId(userId), UserCockSize: userCockSize);
                                          })
                                      .Where(x => x.UserCockSize != null)
                                      .Select(
                                          x =>
                                          {
                                              if (x.UserCockSize.CockSize == null)
                                                  throw new ArgumentNullException(nameof(x.UserCockSize.CockSize),
                                                      $"User cock size is null by {nameof(x.UserId)} {x.UserId}");

                                              if (x.UserId == null)
                                                  throw new ArgumentNullException(nameof(x.UserId),"Cannot parse a user id");

                                              return new UserCockSize
                                              {
                                                  UserId = x.UserId.Value,
                                                  CockSize = x.UserCockSize.CockSize
                                              };
                                          });

            return checkedUsers;
        }

        private static long? ParseUserId(string userIdWithDemoName)
        {
            var splitted = userIdWithDemoName.Split("_");
            var hasSplittedUserId = splitted.Length != 2;
            if (!hasSplittedUserId)
            {
                return null;
            }
            var hasUserId = long.TryParse(splitted[1], out var userId);
            if (!hasUserId)
            {
                return null;
            }
            
            return userId;
        }
    }

    public record UserCockSize
    {
        public long UserId { get; init; }

        public CockSize CockSize { get; init; } = null!;
    }
}