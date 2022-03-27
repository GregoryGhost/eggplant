namespace Lagalike.Demo.Eggplant.MVU.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

    public struct CockSize
    {
        private static readonly Random Random = new();

        public byte Size { get; }

        private CockSize(byte cockSize)
        {
            Size = cockSize;
        }

        public static CockSize GetRandomCockSize()
        {
            const byte MinCockSize = 1;
            const byte MaxCockSize = 50;

            var cockSize = (byte)Random.Next(MinCockSize, MaxCockSize);

            return new CockSize(cockSize);
        }

        public static IReadOnlyList<CockSize> Range()
        {
            return Enumerable.Range(1, 50).Select(x => new CockSize((byte)x)).ToArray();
        }
    }
}