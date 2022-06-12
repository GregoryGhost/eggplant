namespace DudesComparer.Models
{
    public record CheckedDude
    {
        public long UserId { get; init; }
        
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Username { get; init; } = null!;
    }
}