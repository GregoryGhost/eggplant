namespace Eggplant.MVU.CompareDudes.Commands
{
    using DudesComparer.Services;

    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record CompareDudesCommand : BaseCommand<CommandTypes>
    {
        public ComparingDudes? ComparingDudes { get; init; } = null!;
        
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.CompareDudes;
    }
}