namespace GroupRating.Models
{
    public record GroupRatingInfo
    {
        public IReadOnlyCollection<GroupUserInfo> TopUsers { get; init; } = Array.Empty<GroupUserInfo>();
    }
}