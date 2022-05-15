namespace DudesComparer.Models
{
    public record UserCockSize
    {
        public CockSize CockSize { get; init; } = null!;

        public long UserId { get; init; }
    }
}