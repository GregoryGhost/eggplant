namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;

    using Lagalike.Demo.Eggplant.MVU.Services.Domain;

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
        
        public GroupId? GroupId { get; init; }

        /// <inheritdoc />
        public Enum Type => ModelTypes.PersonCockSizeModel;
    }

    public record GroupId
    {
        public string Value { get; init; } = null!;
    }
}