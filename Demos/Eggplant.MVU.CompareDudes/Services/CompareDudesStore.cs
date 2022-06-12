namespace Eggplant.MVU.CompareDudes.Services
{
    using System.Threading.Tasks;

    using DudesComparer.Models;
    using DudesComparer.Services;

    using Lagalike.Telegram.Shared.Services;

    using Telegram.Bot;
    using Telegram.Bot.Types.Enums;
    using TelegramChatId = Telegram.Bot.Types.ChatId;

    public class DudesComparerStore: IDudesComparerStore
    {
        private readonly ICockSizerCache _cockSizerCache;

        private readonly ConfiguredTelegramBotClient _client;

        public DudesComparerStore(ICockSizerCache cockSizerCache, ConfiguredTelegramBotClient client)
        {
            _cockSizerCache = cockSizerCache;
            _client = client;
        }

        public async Task<ChatMember> GetChatMemberAsync(ChatId chatId, string userName)
        {
            var dudeInfo = _cockSizerCache.GetCheckedDude(userName);
            if (dudeInfo is null)
            {
                var emptyUser = GetEmptyUser(userName);

                return emptyUser;
            }
            
            var chatMember = await GetChatMemberAsync(chatId, dudeInfo);

            return chatMember;
        }

        private async Task<ChatMember> GetChatMemberAsync(ChatId chatId, CheckedDude dudeInfo)
        {
            var telegramChatId = new TelegramChatId(chatId.Value);
            var telegramChatMember = await _client.GetChatMemberAsync(telegramChatId, dudeInfo.UserId);
            var isMember =
                telegramChatMember.Status is ChatMemberStatus.Administrator or ChatMemberStatus.Creator or ChatMemberStatus.Member;
            var user = new CheckedDude
            {
                FirstName = telegramChatMember.User.FirstName,
                LastName = telegramChatMember.User.LastName ?? string.Empty,
                Username = telegramChatMember.User.Username ?? string.Empty,
                UserId = telegramChatMember.User.Id
            };
            var chatMember = new ChatMember
            {
                IsMember = isMember,
                User = user
            };
            
            return chatMember;
        }

        private static ChatMember GetEmptyUser(string userName)
        {
            return new ChatMember
            {
                IsMember = false,
                User = new CheckedDude
                {
                    UserId = 0,
                    FirstName = null!,
                    LastName = null!,
                    Username = userName
                }
            };
        }
    }
}