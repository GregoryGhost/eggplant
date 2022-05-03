namespace Eggplant.MVU.GroupRating.Commands
{
    using Eggplant.Types.Shared;

    using global::GroupRating.Models;

    using PatrickStar.MVU;

    public record GroupRatingCommand : BaseCommand<CommandTypes>
    {
        public GroupId GroupId { get; init; } = null!;

        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.GroupRating;
    }
}