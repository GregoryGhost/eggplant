namespace Eggplant.MVU.CompareDudes.Models
{
    using System;

    using DudesComparer.Models;
    using DudesComparer.Services;

    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record CompareDudesModel: IModel
    {
        public ComparedDudes? ComparedDudes { get; init; }
        
        public CheckedDude? CheckedDude { get; init; }
        
        /// <inheritdoc />
        public Enum Type => ModelTypes.CompareDudesModel;
    }
}