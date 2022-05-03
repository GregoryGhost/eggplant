namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;

    using PatrickStar.MVU;

    public record AvailableCommandsModel: IModel
    {
        public string AvailableCommands { get; init; } = null!;

        public Enum Type => ModelTypes.AvailableCommandsModel;
    }
}