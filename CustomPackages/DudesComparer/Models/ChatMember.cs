namespace DudesComparer.Models
{
    public record ChatMember
    {
        public bool IsMember { get; init; }

        public CheckedDude User { get; init; } = null!;
    }
}