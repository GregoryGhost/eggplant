namespace GroupRating.Models
{
    using System;
    using System.Collections.Generic;

    public record GroupRatingInfo
    {
        public IReadOnlyCollection<GroupUserInfo> TopUsers { get; init; } = Array.Empty<GroupUserInfo>();
    }
}