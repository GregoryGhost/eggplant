namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;

    using PatrickStar.MVU;

    public record MessageWithoutAnyCmdModel : IModel
    {
        /// <inheritdoc />
        public Enum Type => ModelTypes.MessageWithoutAnyCmdModel;
    }
}