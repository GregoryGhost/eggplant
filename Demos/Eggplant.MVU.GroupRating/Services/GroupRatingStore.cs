namespace Eggplant.MVU.GroupRating.Services
{
    using global::GroupRating.Models;
    using global::GroupRating.Services;

    using Lagalike.Telegram.Shared.Services;

    using TelegramChatId = Telegram.Bot.Types.ChatId;

    public class GroupRatingStore : IGroupRatingStore
    {
        private readonly ConfiguredTelegramBotClient _client;

        public GroupRatingStore(ConfiguredTelegramBotClient client)
        {
            _client = client;
        }

        public async Task<ChatMember> GetChatMemberAsync(ChatId chatId, long userId)
        {
            var telegramChatId = new TelegramChatId(chatId.Value);

            var telegramChatMember = await _client.GetChatMemberAsync(telegramChatId, userId);
            var isMember =
                telegramChatMember.Status is ChatMemberStatus.Administrator or ChatMemberStatus.Creator
                    or ChatMemberStatus.Member;
            var user = new UserInfo
            {
                FirstName = telegramChatMember.User.FirstName,
                LastName = telegramChatMember.User.LastName ?? string.Empty,
                Username = telegramChatMember.User.Username ?? string.Empty
            };
            var chatMember = new ChatMember
            {
                IsMember = isMember,
                User = user
            };

            return chatMember;
        }
    }
}