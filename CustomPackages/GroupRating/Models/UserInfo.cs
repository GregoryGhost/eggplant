namespace GroupRating.Models
{
    public record UserInfo
    {
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Username { get; init; } = null!;
    }
}