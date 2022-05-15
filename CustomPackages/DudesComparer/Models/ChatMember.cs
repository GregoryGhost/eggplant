namespace DudesComparer.Models
{
    public record ChatMember
    {
        public bool IsMember { get; init; }

        public UserInfo User { get; init; } = null!;
    }
}