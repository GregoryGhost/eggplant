namespace DudesComparer.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CSharpFunctionalExtensions;

    using DudesComparer.Models;
    using DudesComparer.Services;

    using FluentAssertions;
    using FluentAssertions.CSharpFunctionalExtensions;

    using NUnit.Framework;

    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCaseSource(typeof(CompareDudesCases))]
        public async Task CompareDudesTestAsync(string testCaseName, ComparingDudes inputData,
            Result<ComparedDudes, ComparedDudesErrors> expectedResult)
        {
            var dudesHandler = DudesHandlerFactory.GetInitializedDudesHandler();
            var actualComparedDudes = await dudesHandler.CompareDudesAsync(inputData);

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
    }

    public record CompareDudeTestCase
    {
        public Result<ComparedDudes, ComparedDudesErrors> Expected { get; init; }

        public ComparingDudes InputData { get; init; } = null!;

        public string TestCaseName { get; init; } = null!;
    }

    internal class CompareDudesCases : IEnumerable
    {
        private static readonly IReadOnlyCollection<CompareDudeTestCase> TestCases = new[]
        {
            GetCompareTwoDudesCompareTestCase(),
            GetCompareManyDudesCompareTestCase(),
            GetCompareOnEmptyChatMembersTestCase()
        };

        public IEnumerator GetEnumerator()
        {
            return TestCases.Select(x => new object[] {x.TestCaseName, x.InputData, x.Expected})
                            .GetEnumerator();
        }

        private static CompareDudeTestCase GetCompareManyDudesCompareTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetCompareManyDudesCompareTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChatId,
                    DudesUserNames = FakeDudesStore.GetChatUsernames(FakeDudesStore.TestChatId)
                                                   .ToArray()
                },
                Expected = new ComparedDudes
                {
                    DudeInfos = new DudeInfo[]
                        {
                            new()
                            {
                                DudeType = DudeTypes.Winner,
                                CockSize = new CockSize(10),
                                UserInfo = FakeDudesStore.GetUserInfoById(1)
                            },
                        }.Concat(
                             FakeCockSizerCache.GetUserCockSizes()
                                               .Skip(1)
                                               .Select(
                                                   x => new DudeInfo
                                                   {
                                                       DudeType = DudeTypes.Loser,
                                                       CockSize = x.CockSize,
                                                       UserInfo = FakeDudesStore.GetUserInfoById(x.UserId)
                                                   }))
                         .ToList()
                }
            };
        }

        private static CompareDudeTestCase GetCompareOnEmptyChatMembersTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetCompareOnEmptyChatMembersTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChatId,
                    DudesUserNames = new [] { "322", "228", "888" }
                },
                Expected = ComparedDudesErrors.EmptyChatDudes
            };
        }

        private static CompareDudeTestCase GetCompareTwoDudesCompareTestCase()
        {
            return new CompareDudeTestCase
            {
                TestCaseName = nameof(GetCompareTwoDudesCompareTestCase),
                InputData = new ComparingDudes
                {
                    ChatId = FakeDudesStore.TestChatId,
                    DudesUserNames = FakeDudesStore.GetChatUsernames(FakeDudesStore.TestChatId)
                                                   .Take(2)
                                                   .ToArray()
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
                            CockSize = new CockSize(9),
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