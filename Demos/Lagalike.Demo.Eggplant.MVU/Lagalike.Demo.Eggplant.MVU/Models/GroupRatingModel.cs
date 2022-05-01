namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;
    using System.Collections.Generic;

    using Lagalike.Demo.Eggplant.MVU.Services.Views;

    using PatrickStar.MVU;

    public record GroupRatingModel: IModel
    {
        public IReadOnlyList<GroupRatingInfo> GroupRatings { get; init; } = Array.Empty<GroupRatingInfo>();
        public Enum Type => ModelTypes.GroupRatingModel;
    }
}