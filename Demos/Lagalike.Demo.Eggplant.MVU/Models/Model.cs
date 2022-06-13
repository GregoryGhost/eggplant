namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using global::Eggplant.MVU.CompareDudes.Models;
    using global::Eggplant.MVU.GroupRating.Models;
    using global::Eggplant.MVU.ShareCockSize.Models;
    using global::Eggplant.MVU.UnknownCmd.Models;
    using global::Eggplant.Types.Shared;

    /// <summary>
    ///     Data model.
    /// </summary>
    public record Model : IModel
    {
        public AvailableCommandsModel? AvailableCommandsModel { get; init; }

        /// <summary>
        ///     Current user cock size for a user demo session.
        /// </summary>
        public PersonCockSizeModel? CockSizeModel { get; init; }

        public CompareDudesModel? CompareDudesModel { get; init; }

        public CommandTypes CurrentCommand { get; init; } = CommandTypes.UnknownCommand;

        public GroupRatingModel? GroupRatingModel { get; init; }

        /// <inheritdoc />
        public Enum Type => ModelTypes.RootModel;
    }
}