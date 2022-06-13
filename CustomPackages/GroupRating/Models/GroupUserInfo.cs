namespace GroupRating.Models
{
    public record GroupUserInfo
    {
        public CockSize CockSize { get; init; } = null!;

        public string FullName { get; init; } = null!;
    }
}