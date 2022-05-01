namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;

    using Lagalike.Demo.Eggplant.MVU.Services.Domain;

    using PatrickStar.MVU;

    public record PersonCockSizeModel: IModel
    {
        public CockSize? CockSize { get; init; }

        public Enum Type => ModelTypes.PersonCockSizeModel;
    }
}