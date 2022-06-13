namespace Eggplant.MVU.CompareDudes.Models
{
    using DudesComparer.Models;
    using DudesComparer.Services;

    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record CompareDudesModel : IModel
    {
        public CheckedDude? CheckedDude { get; init; }

        public Result<ComparedDudes, ComparedDudesErrors>? ComparedDudes { get; init; }

        /// <inheritdoc />
        public Enum Type => ModelTypes.CompareDudesModel;
    }
}