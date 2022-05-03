namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;

    using Lagalike.Demo.Eggplant.MVU.Commands;

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
        
        public AvailableCommandsModel? AvailableCommandsModel { get; set; }

        public CommandTypes CurrentCommand { get; init; } = CommandTypes.UnknownCommand;

        /// <inheritdoc />
        public Enum Type => ModelTypes.RootModel;
    }
}