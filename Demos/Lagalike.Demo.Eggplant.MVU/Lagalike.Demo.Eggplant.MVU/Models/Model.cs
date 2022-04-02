namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;

    using Lagalike.Demo.Eggplant.MVU.Services.Domen;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    /// <summary>
    ///     Data model.
    /// </summary>
    public record Model : IModel
    {
        /// <summary>
        ///     Current user cock size for a user demo session.
        /// </summary>
        public CockSize? CockSize { get; init; }

        /// <inheritdoc />
        public Enum Type => ModelTypes.DefaultModel;
    }
}