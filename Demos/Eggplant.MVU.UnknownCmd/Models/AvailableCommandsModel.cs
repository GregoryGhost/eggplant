namespace Eggplant.MVU.UnknownCmd.Models
{
    using System;

    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record AvailableCommandsModel: IModel
    {
        public string AvailableCommands { get; init; } = null!;

        public Enum Type => ModelTypes.AvailableCommandsModel;
    }
}