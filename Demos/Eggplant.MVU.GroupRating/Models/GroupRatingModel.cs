namespace Eggplant.MVU.GroupRating.Models
{
    using System;

    using Eggplant.Types.Shared;

    using global::GroupRating.Models;
    using global::GroupRating.Services;

    using PatrickStar.MVU;

    public record GroupRatingModel: IModel
    {
        public GroupRatingInfo? GroupRating { get; init; }
        public Enum Type => ModelTypes.GroupRatingModel;
    }
}