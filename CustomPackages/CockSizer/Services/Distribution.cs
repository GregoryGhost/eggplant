﻿namespace CockSizer.Services
{
    using MathNet.Numerics.Distributions;

    public class Distribution: IDistribution
    {
        private readonly Gamma _gammaDistribution;
        
        public Distribution()
        {
            _gammaDistribution = new Gamma(6, 4);
        }

        public double Sample()
        {
            return _gammaDistribution.Sample();
        }
    }
}