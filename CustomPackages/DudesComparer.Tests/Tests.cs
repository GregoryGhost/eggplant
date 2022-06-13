namespace DudesComparer.Tests
{
    using DudesComparer.Models;
    using DudesComparer.Services;

    using Random = FsCheck.Random;

    [TestFixture]
    public class Tests
    {
        private readonly IDudesHandler _dudesHandler;

        public Tests()
        {
            _dudesHandler = DudesHandlerFactory.GetInitializedDudesHandler();
        }

        [Test]
        public void CanCompareDudesTest()
        {
            var userCockSizes = GenUserCockSize();
            Prop.ForAll(
                    userCockSizes,
                    size => { })
                .QuickCheck();
        }

        [TestCaseSource(typeof(CompareDudesCases))]
        public async Task CompareDudesTestAsync(string testCaseName, ComparingDudes? inputData,
            Result<ComparedDudes, ComparedDudesErrors> expectedResult)
        {
            var actualComparedDudes = await _dudesHandler.CompareDudesAsync(inputData);

            expectedResult.Match(
                expected =>
                {
                    actualComparedDudes.Should().BeSuccess();
                    expected.Should().BeEquivalentTo(actualComparedDudes.Value);
                },
                expected =>
                {
                    actualComparedDudes.Should().BeFailure();
                    expected.Should().BeEquivalentTo(actualComparedDudes.Error);
                });
        }

        private static Arbitrary<UserCockSize> GenUserCockSize()
        {
            return Gen.Elements(DataFaker.GetUserCockSizes())
                      .ToArbitrary();
        }
    }
}