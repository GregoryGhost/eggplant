namespace DudesComparer.Tests
{
    using CSharpFunctionalExtensions;

    using DudesComparer.Services;

    public record CompareDudeTestCase
    {
        public Result<ComparedDudes, ComparedDudesErrors> Expected { get; init; }

        public ComparingDudes InputData { get; init; } = null!;

        public string TestCaseName { get; init; } = null!;
    }
}