namespace Lagalike.Demo.Eggplant.MVU.Services.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MathNet.Numerics.Distributions;

    public class CockSizeFactory
    {
        private readonly Gamma _gammaDistribution;

        private const byte MIN_COCK_SIZE = 1;

        private const byte MAX_COCK_SIZE = 50;

        public CockSizeFactory(Gamma gammaDistribution)
        {
            _gammaDistribution = gammaDistribution;
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
            var sample = Math.Ceiling(_gammaDistribution.Sample() * 10);
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

    public record CockSize(byte Size);
}