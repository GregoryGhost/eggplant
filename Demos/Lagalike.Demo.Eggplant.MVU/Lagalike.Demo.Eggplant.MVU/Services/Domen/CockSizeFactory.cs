namespace Lagalike.Demo.Eggplant.MVU.Services.Domen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CockSizeFactory
    {
        private readonly Random _random;

        private const byte MIN_COCK_SIZE = 1;

        private const byte MAX_COCK_SIZE = 50;

        public CockSizeFactory(Random random)
        {
            _random = random;
        }
        
        public CockSize GetRandomCockSize()
        {
            var cockSize = (byte)_random.Next(MIN_COCK_SIZE, MAX_COCK_SIZE);

            return new CockSize(cockSize);
        }

        public IReadOnlyList<CockSize> GetCockSizeRanges()
        {
            return Enumerable.Range(MIN_COCK_SIZE, MAX_COCK_SIZE)
                             .Select(x => new CockSize((byte)x))
                             .ToArray();
        }
    }

    public record CockSize(byte Size);
}