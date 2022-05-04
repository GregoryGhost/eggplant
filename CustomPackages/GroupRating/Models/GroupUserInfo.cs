namespace GroupRating.Models
{
    public record GroupUserInfo
    {
        public string FullName { get; init; } = null!;

        public CockSize CockSize { get; init; } = null!;
    }
}