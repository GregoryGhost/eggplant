namespace DudesComparer.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DudesComparer.Models;
    using DudesComparer.Services;

    using FluentAssertions;

    using NUnit.Framework;

    public class Tests
    {
        [TestCaseSource(typeof(CompareDudesCases))]
        public async Task CompareDudesTestAsync(string testCaseName, ComparingDudes inputData, ComparedDudes expected)
        {
            var dudesHandler = DudesHandlerFactory.GetInitializedDudesHandler();
            var actualComparedDudes = await dudesHandler.CompareDudesAsync(inputData);

            expected.Should().BeEquivalentTo(actualComparedDudes);
        }

        [SetUp]
        public void Setup()
        {
        }
    }

    internal record CompareDudeTestCase
    {
        public string TestCaseName { get; init; } = null!;
        public ComparedDudes Expected { get; init; } = null!;

        public ComparingDudes InputData { get; init; } = null!;
    }

    internal class CompareDudesCases : IEnumerable
    {
        private static readonly IReadOnlyCollection<CompareDudeTestCase> TestCases = new[]
        {
            GetCompareTwoDudesCompareTestCase()
        };

        public IEnumerator GetEnumerator()
        {
            return TestCases.Select(x => new object[] {x.TestCaseName, x.InputData, x.Expected})
                            .GetEnumerator();
        }

        private static CompareDudeTestCase GetCompareTwoDudesCompareTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetCompareTwoDudesCompareTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = new ChatId("testChatId"),
                    DudesUserNames = new[] {"dude1", "dude2"}
                },
                Expected = new ComparedDudes
                {
                    DudeInfos = new DudeInfo[]
                    {
                        new()
                        {
                            DudeType = DudeTypes.Winner,
                            CockSize = new CockSize(10),
                            UserInfo = new UserInfo
                            {
                                UserId = 1,
                                FirstName = "dude1FN",
                                LastName = "dude1LN",
                                Username = "dude1"
                            }
                        },
                        new()
                        {
                            DudeType = DudeTypes.Loser,
                            CockSize = new CockSize(5),
                            UserInfo = new UserInfo
                            {
                                UserId = 2,
                                FirstName = "dude2FN",
                                LastName = "dude2LN",
                                Username = "dude2"
                            }
                        }
                    }
                }
            };
        }
    }
}