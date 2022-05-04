namespace CockSizer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CockSizer.Models;

    public class CockSizeFactory
    {
        private readonly IDistribution _distribution;

        private const byte MIN_COCK_SIZE = 1;

        private const byte MAX_COCK_SIZE = 50;

        public CockSizeFactory(IDistribution distribution)
        {
            _distribution = distribution;
        }
        
        public CockSize GetRandomCockSize()
        {
            var cockSize = GetWeightedRandom();

            return new CockSize(cockSize);
        }
        
        public IReadOnlyList<CockSize> GetCockSizeRanges()
        {
            return Enumerable.Range(MIN_COCK_SIZE, MAX_COCK_SIZE)
                             .Select(x => new CockSize((byte)x))
                             .ToArray();
        }

        private byte GetWeightedRandom()
        {
            GetSample:
            var sample = Math.Ceiling(_distribution.Sample() * 10);
            if (sample is >= MIN_COCK_SIZE and <= MAX_COCK_SIZE)
            {
                return (byte)sample;
            }
            else
            {
                goto GetSample;
            }
        }
    }
}