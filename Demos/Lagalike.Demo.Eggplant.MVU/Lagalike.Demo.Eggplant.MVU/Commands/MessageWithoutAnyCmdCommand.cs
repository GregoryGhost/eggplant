namespace Lagalike.Demo.Eggplant.MVU.Commands
{
    using PatrickStar.MVU;

    public record MessageWithoutAnyCmdCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.MessageWithoutAnyCmdCommand;
    };
}