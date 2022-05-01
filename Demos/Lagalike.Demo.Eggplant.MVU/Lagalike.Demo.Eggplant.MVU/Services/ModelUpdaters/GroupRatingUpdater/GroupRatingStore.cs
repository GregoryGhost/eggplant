namespace Lagalike.Demo.Eggplant.MVU.Services.ModelUpdaters
{
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.Caching.Memory;

    public class GroupRatingStore : BaseTelegramBotCache<GroupRatingInfo>
    {
        private const string DEMO_NAME = "group-rating-cock-size";

        public GroupRatingStore(IMemoryCache telegramCache)
            : base(telegramCache, DEMO_NAME)
        {
        }
    }
}