namespace Eggplant.MVU.ShareCockSize.Models
{
    using System;

    using CockSizer.Models;

    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record PersonCockSizeModel: IModel
    {
        public CockSize? CockSize { get; init; } = null;

        public Enum Type => ModelTypes.PersonCockSizeModel;
    }
}