namespace DudesComparer.Services
{
    using DudesComparer.Models;

    public interface IDudesComparerStore
    {
        Task<ChatMember> GetChatMemberAsync(ChatId chatId, string userName);
    }
}