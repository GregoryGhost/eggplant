namespace Eggplant.MVU.CompareDudes.Models
{
    using System;

    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record CompareDudesModel: IModel
    {
        public ComparingDudes? ComparingDudes { get; set; }
        
        /// <inheritdoc />
        public Enum Type => ModelTypes.CompareDudesModel;
    }
}