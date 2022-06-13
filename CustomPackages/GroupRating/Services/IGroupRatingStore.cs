namespace GroupRating.Services
{
    using GroupRating.Models;

    public interface IGroupRatingStore
    {
        Task<ChatMember> GetChatMemberAsync(ChatId chatId, long userId);
    }
}