namespace Eggplant.MVU.MessageWithoutAnyCmd.Commands
{
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record MessageWithoutAnyCmdCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.MessageWithoutAnyCmdCommand;
    };
}