namespace DudesComparer.Services
{
    using System.Threading.Tasks;

    using DudesComparer.Models;

    public interface IDudesComparerStore
    {
        Task<ChatMember> GetChatMemberAsync(ChatId chatId, string userName);
    }
}