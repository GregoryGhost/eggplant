namespace Eggplant.MVU.CompareDudes.Commands
{
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record CompareDudesCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.CompareDudes;
    }
}