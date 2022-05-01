using PatrickStar.MVU;

namespace Lagalike.Demo.Eggplant.MVU.Commands
{
    using Lagalike.Demo.Eggplant.MVU.Models;

    public record GroupRatingCommand : BaseCommand<CommandTypes>
    {
        public GroupId GroupId { get; init; } = null!;

        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.GroupRating;
    }
}