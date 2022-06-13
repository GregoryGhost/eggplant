namespace DudesComparer.Models
{
    public record CheckedDude
    {
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public long UserId { get; init; }

        public string Username { get; init; } = null!;
    }
}