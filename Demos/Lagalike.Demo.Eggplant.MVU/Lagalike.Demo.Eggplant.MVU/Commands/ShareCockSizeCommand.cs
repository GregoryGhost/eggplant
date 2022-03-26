namespace Lagalike.Demo.Eggplant.MVU.Commands
{
    using PatrickStar.MVU;

    /// <summary>
    /// Command to share random current user cock size with other users.
    /// </summary>
    public record ShareCockSizeCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.ShareCockSize;
    }
}