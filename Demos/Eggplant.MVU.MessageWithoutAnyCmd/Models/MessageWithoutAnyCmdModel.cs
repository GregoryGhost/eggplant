namespace Eggplant.MVU.MessageWithoutAnyCmd.Models
{
    using System;

    using Eggplant.Types.Shared;

    using PatrickStar.MVU;

    public record MessageWithoutAnyCmdModel : IModel
    {
        /// <inheritdoc />
        public Enum Type => ModelTypes.MessageWithoutAnyCmdModel;
    }
}