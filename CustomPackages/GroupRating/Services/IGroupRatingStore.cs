namespace GroupRating.Services
{
    using System.Threading.Tasks;

    using GroupRating.Models;

    public interface IGroupRatingStore
    {
        Task<ChatMember> GetChatMemberAsync(ChatId chatId, long userId);
    }
}