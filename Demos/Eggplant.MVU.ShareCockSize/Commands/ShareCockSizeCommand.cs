namespace Eggplant.MVU.ShareCockSize.Commands
{
    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    /// <summary>
    ///     Command to share random current user cock size with other users.
    /// </summary>
    public record ShareCockSizeCommand : BaseCommand<CommandTypes>
    {
        public string ChatId { get; init; } = null!;

        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.ShareCockSize;
    }
}