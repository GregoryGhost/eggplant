namespace Eggplant.MVU.UnknownCmd.Commands
{
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    /// <summary>
    ///     It's service command to singal that a input command is not parsed - unknown.
    /// </summary>
    public record UnknownCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.UnknownCommand;
    }
}