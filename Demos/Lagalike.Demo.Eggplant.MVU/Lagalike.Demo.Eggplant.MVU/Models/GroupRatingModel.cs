namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;
    using System.Collections.Generic;

    using Lagalike.Demo.Eggplant.MVU.Services.ModelUpdaters;
    using Lagalike.Demo.Eggplant.MVU.Services.Views;

    using PatrickStar.MVU;

    public record GroupRatingModel: IModel
    {
        public GroupRatingInfo? GroupRating { get; init; }
        public Enum Type => ModelTypes.GroupRatingModel;
    }
    
    
    public record GroupId
    {
        public string Value { get; init; } = null!;
    }
}