namespace CockSizer.Services
{
    using CockSizer.Models;

    public class CockSizeFactory
    {
        private const byte MAX_COCK_SIZE = 50;

        private const byte MIN_COCK_SIZE = 1;

        private readonly IDistribution _distribution;

        public CockSizeFactory(IDistribution distribution)
        {
            _distribution = distribution;
        }

        public IReadOnlyList<CockSize> GetCockSizeRanges()
        {
            return Enumerable.Range(MIN_COCK_SIZE, MAX_COCK_SIZE)
                             .Select(x => new CockSize((byte)x))
                             .ToArray();
        }

        public CockSize GetRandomCockSize()
        {
            var cockSize = GetWeightedRandom();

            return new CockSize(cockSize);
        }

        private byte GetWeightedRandom()
        {
            GetSample:
            var sample = Math.Ceiling(_distribution.Sample() * 10);
            if (sample is >= MIN_COCK_SIZE and <= MAX_COCK_SIZE)
                return (byte)sample;
            goto GetSample;
        }
    }
}