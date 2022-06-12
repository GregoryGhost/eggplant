namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;

    using DudesComparer.Models;

    using global::Eggplant.MVU.CompareDudes.Models;
    using global::Eggplant.MVU.GroupRating.Models;
    using global::Eggplant.MVU.ShareCockSize.Models;
    using global::Eggplant.MVU.UnknownCmd.Models;
    using global::Eggplant.Types.Shared;

    using PatrickStar.MVU;

    /// <summary>
    ///     Data model.
    /// </summary>
    public record Model : IModel
    {
        /// <summary>
        ///     Current user cock size for a user demo session.
        /// </summary>
        public PersonCockSizeModel? CockSizeModel { get; init; }
        
        public GroupRatingModel? GroupRatingModel { get; init; }
        
        public AvailableCommandsModel? AvailableCommandsModel { get; init; }
        
        public CompareDudesModel? CompareDudesModel { get; init; }

        public CommandTypes CurrentCommand { get; init; } = CommandTypes.UnknownCommand;
        

        /// <inheritdoc />
        public Enum Type => ModelTypes.RootModel;
    }
}